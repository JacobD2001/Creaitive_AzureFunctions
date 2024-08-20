using Creaitive.RealEstateLeadAgent.Models.InsertRows;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace Creaitive.RealEstateLeadAgent.Functions
{
    public class InsertRows
    {
        private const string apifyToken = "apify_api_ct06M4KKZTqMyqBxky9cBpPxXQSyS11di9cl";

        private readonly ILogger<InsertRows> _logger;

        public InsertRows(ILogger<InsertRows> logger)
        {
            _logger = logger;
        }

        [Function("InsertRows")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Function invoked.");

            try
            {
                // Parse the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<RequestData>(requestBody);

                _logger.LogInformation("Received request with body: {Body}", requestBody);

                // Validate required fields
                if (data == null || string.IsNullOrEmpty(data.AirtableBaseId) || string.IsNullOrEmpty(data.AirtablePersonalToken) || string.IsNullOrEmpty(data.TableIdOrName))
                {
                    _logger.LogError("Missing required fields: {Data}", data);
                    return new BadRequestObjectResult(new { error = "Missing required fields" });
                }

                // Fetch data from external API
                var apiUrl = $"https://api.apify.com/v2/acts/jupri~realtor-agents/runs/last/dataset/items?status=SUCCEEDED&token={apifyToken}";
                var httpClient = new HttpClient();
                var apiResponse = await httpClient.GetStringAsync(apiUrl);

                _logger.LogInformation("Fetched the following data from APIFY: ", JsonConvert.SerializeObject(apiResponse));


                // Deserialize the response into our model
                var fetchedData = JsonConvert.DeserializeObject<List<DataToInsert>>(apiResponse);

                // Prepare the records for Airtable
                var records = new List<Record>();

                foreach (var item in fetchedData)
                {
                    var fields = new Fields
                    {
                        // Agent Information
                        Email = item.Email,
                        EmailContent = null,  
                        Status = "pending",  
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        FullName = item.FullName,
                        Role = item.Role,
                        Title = item.Title,
                        PhoneMobile = item.Phones?.FirstOrDefault(p => p.Type == "Mobile")?.Number,
                        AgentRating = item.AgentRating,
                        Description = item.Description,
                        Website = item.WebUrl,
                        LastUpdated = !string.IsNullOrEmpty(item.LastUpdated) ? DateTime.Parse(item.LastUpdated) : (DateTime?)null,

                        // Agent Address
                        AddressCity = item.Address?.City,
                        AddressLine = item.Address?.Line,
                        AddressPostalCode = item.Address?.PostalCode,
                        AddressState = item.Address?.State,

                        // Office Information
                        OfficeName = item.Office?.Name,
                        OfficeSlogan = item.Slogan,  
                        OfficeWebsite = item.Office?.Website,
                        OfficeEmail = item.Office?.Email,
                        OfficeAddressCity = item.Office?.Address?.City,
                        OfficeAddressLine = item.Office?.Address?.Line,
                        OfficeAddressPostalCode = item.Office?.Address?.PostalCode,
                        OfficeAddressState = item.Office?.Address?.StateCode,
                        OfficePhone = item.Office?.Phones?.FirstOrDefault()?.Number,  

                        // For Sale Information
                        ForSaleCount = item.ForSalePrice?.Count,
                        ForSaleLastListingDate = !string.IsNullOrEmpty(item.ForSalePrice?.LastListingDate) ? DateTime.Parse(item.ForSalePrice.LastListingDate) : (DateTime?)null,
                        ForSaleMaxPrice = item.ForSalePrice?.Max,
                        ForSaleMinPrice = item.ForSalePrice?.Min,

                        // Recently Sold Information
                        RecentlySoldLastSoldDate = !string.IsNullOrEmpty(item.RecentlySold?.LastSoldDate) ? DateTime.Parse(item.RecentlySold.LastSoldDate) : (DateTime?)null,
                        RecentlySoldMaxPrice = item.RecentlySold?.Max,
                        RecentlySoldMinPrice = item.RecentlySold?.Min,

                        // Specializations (only first three)
                        Specialization1Name = item.Specializations?.ElementAtOrDefault(0)?.Name,
                        Specialization2Name = item.Specializations?.ElementAtOrDefault(1)?.Name,
                        Specialization3Name = item.Specializations?.ElementAtOrDefault(2)?.Name,

                        // Marketing Cities (only first three)
                        MarketingCity1Name = item.MarketingAreaCities?.ElementAtOrDefault(0)?.Name,
                        MarketingCity2Name = item.MarketingAreaCities?.ElementAtOrDefault(1)?.Name,
                        MarketingCity3Name = item.MarketingAreaCities?.ElementAtOrDefault(2)?.Name,
                    };

                    records.Add(new Record { Fields = fields });
                }



                _logger.LogInformation("Records to be added: {Records}", JsonConvert.SerializeObject(records));

                const int chunkSize = 10;
                int chunkCount = 0;

                for (int i = 0; i < records.Count; i += chunkSize)
                {
                    var chunk = records.GetRange(i, Math.Min(chunkSize, records.Count - i));
                    chunkCount++;

                    _logger.LogInformation("Processing chunk {ChunkCount}: {Chunk}", chunkCount, JsonConvert.SerializeObject(chunk));

                    try
                    {
                        var options = new RestClientOptions($"https://api.airtable.com/v0/{data.AirtableBaseId}/{data.TableIdOrName}")
                        {
                            Timeout = TimeSpan.FromMilliseconds(30000)
                        };

                        var client = new RestClient(options);
                        var request = new RestRequest("", Method.Post);

                        request.AddHeader("Authorization", $"Bearer {data.AirtablePersonalToken}");
                        request.AddHeader("Content-Type", "application/json");

                        var jsonSettings = new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented
                        };

                        var jsonBody = JsonConvert.SerializeObject(new { records = chunk }, jsonSettings);
                        request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

                        _logger.LogInformation("Airtable request body: {jsonBody}", jsonBody);

                        var response = await client.ExecuteAsync(request);

                        if (!response.IsSuccessful)
                        {
                            _logger.LogError("Error adding chunk {ChunkCount}: {ResponseContent}", chunkCount, response.Content);
                            return new StatusCodeResult((int)response.StatusCode);
                        }

                        _logger.LogInformation("Chunk {ChunkCount} added successfully: {ResponseContent}", chunkCount, response.Content);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception occurred while processing chunk {ChunkCount}", chunkCount);
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }

                    _logger.LogInformation("Pausing for 3 seconds before processing the next chunk...");
                    await Task.Delay(3000);
                }

                _logger.LogInformation("All chunks processed successfully.");
                return new OkObjectResult(new { message = "All rows added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in function execution");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

