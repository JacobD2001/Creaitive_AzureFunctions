using Creaitive.RealEstateLeadAgent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Functions
{
    public class InsertRowsWithGptCompletion
    {
        private readonly ILogger<InsertRowsWithGptCompletion> _logger;

        public InsertRowsWithGptCompletion(ILogger<InsertRowsWithGptCompletion> logger)
        {
            _logger = logger;
        }

        [Function("InsertRowsWithGptCompletion")]
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
                if (data == null || data.VerifiedEmails == null || data.EmailContent == null ||
                    string.IsNullOrEmpty(data.AirtableBaseId) || string.IsNullOrEmpty(data.AirtablePersonalToken) ||
                    string.IsNullOrEmpty(data.TableIdOrName))
                {
                    _logger.LogError("Missing required fields: {Data}", data);
                    return new BadRequestObjectResult(new { error = "Missing required fields" });
                }

                if (data.VerifiedEmails.Count != data.EmailContent.Count)
                {
                    _logger.LogError("Mismatch between number of emails and email contents: Emails = {EmailCount}, EmailContents = {EmailContentCount}",
                        data.VerifiedEmails.Count, data.EmailContent.Count);
                    return new BadRequestObjectResult(new { error = "The number of emails and email contents must match" });
                }

                // Prepare the rows to be added to Airtable
                var records = new List<Record>();
                for (int i = 0; i < data.VerifiedEmails.Count; i++)
                {
                    records.Add(new Record
                    {
                        Fields = new Fields
                        {
                            Email = data.VerifiedEmails[i],
                            EmailContent = data.EmailContent[i],
                            Status = "pending"
                        }
                    });
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
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            Formatting = Formatting.Indented
                        };

                        var jsonBody = JsonConvert.SerializeObject(new { records = chunk }, jsonSettings);
                        request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

                        _logger.LogInformation("airtable request body: {jsonBody}", jsonBody);

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
