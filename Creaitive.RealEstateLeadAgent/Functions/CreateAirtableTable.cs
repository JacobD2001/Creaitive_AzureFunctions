using Creaitive.RealEstateLeadAgent.Models;
using Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable;
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
    public class CreateAirtableTable
    {
        private readonly ILogger<CreateAirtableTable> _logger;

        public CreateAirtableTable(ILogger<CreateAirtableTable> logger)
        {
            _logger = logger;
        }

        [Function("CreateAirtableTable")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("CreateAirtableTable function invoked.");

            try
            {
                // Parse the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<RequestData>(requestBody);

                _logger.LogInformation("Received request with body: {Body}", requestBody);

                // Validate required fields
                if (string.IsNullOrEmpty(data.AirtableBaseId) || string.IsNullOrEmpty(data.AirtablePersonalToken) || string.IsNullOrEmpty(data.TableName))
                {
                    _logger.LogError("Missing required fields: {Data}", data);
                    return new BadRequestObjectResult(new { error = "baseId, apiKey, and tableName are required" });
                }

                // Define the fields for the new table
                var fields = new List<AirtableField>
                {
                    new AirtableField { Name = "Agent-Email", Type = "email" },
                    new AirtableField { Name = "E-mail-Content", Type = "richText" },
                    new AirtableField
                    {
                        Name = "Status",
                        Type = "singleSelect",
                        Options = new SingleSelectOptions
                        {
                            Choices = new List<FieldChoice>
                            {
                                new FieldChoice { Name = "pending" },
                                new FieldChoice { Name = "sent" }
                            }
                        }
                    },                   
                    new AirtableField { Name = "Agent-First-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Last-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Full-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Role", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Title", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Phone-Office", Type = "phoneNumber" },
                    new AirtableField { Name = "Agent-Phone-Mobile", Type = "phoneNumber" },
                    new AirtableField
                    {
                        Name = "Agent-Rating",
                        Type = "rating",
                        Options = new RatingOptions
                        {
                            Color = "yellowBright", 
                            Icon = "star", 
                            Max = 5 
                        }
                    },
                    new AirtableField { Name = "Agent-Description", Type = "richText" },
                    new AirtableField { Name = "Agent-Website", Type = "url" },
                    new AirtableField
                    {
                        Name = "Agent-Last-Updated",
                        Type = "dateTime",
                        Options = new DateTimeOptions
                        {
                            TimeZone = "utc",
                            DateFormat = new AirtableDateFormat
                            {
                                Format = "YYYY-MM-DD",
                                Name = "iso",
                            },
                            TimeFormat = new AirtableTimeFormat
                            {
                                Name = "24hour",
                                Format = "HH:mm"
                            },
                        }
                    },              
                    // Agent Address
                    new AirtableField { Name = "Agent-Address-City", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Address-Country", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Address-Line", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Address-Postal-Code", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Address-State-Code", Type = "singleLineText" },

                    // Office Information
                    new AirtableField { Name = "Office-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Slogan", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Website", Type = "url" },
                    new AirtableField { Name = "Office-Email", Type = "email" },

                    // Office Address
                    new AirtableField { Name = "Office-Address-City", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Address-Country", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Address-Line", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Address-Postal-Code", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Address-State-Code", Type = "singleLineText" },

                    // Office Phones
                    new AirtableField { Name = "Office-Phone-1", Type = "phoneNumber" },
                    new AirtableField { Name = "Office-Phone-1-Type", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Phone-2", Type = "phoneNumber" },
                    new AirtableField { Name = "Office-Phone-2-Type", Type = "singleLineText" },
                    new AirtableField { Name = "Office-Phone-3", Type = "phoneNumber" },
                    new AirtableField { Name = "Office-Phone-3-Type", Type = "singleLineText" },

                    // For Sale Information
                    new AirtableField {
                        Name = "For-Sale-Count",
                        Type = "number",
                        Options = new NumberOptions {
                            Precision = 0
                        }
                    },
                    new AirtableField { Name = "For-Sale-Last-Listing-Date",
                        Type = "dateTime",
                        Options = new DateTimeOptions
                        {
                            TimeZone = "utc",
                            DateFormat = new AirtableDateFormat
                            {
                                Format = "YYYY-MM-DD",
                                Name = "iso",
                            },
                            TimeFormat = new AirtableTimeFormat
                            {
                                Name = "24hour",
                                Format = "HH:mm"
                            },
                        }
                    },
                    new AirtableField {
                        Name = "For-Sale-Max-Price",
                        Type = "currency",
                        Options = new CurrencyOptions {
                            Precision = 0, 
                            Symbol = "$"   
                        }
                    },
                    new AirtableField { Name = "For-Sale-Min-Price",
                        Type = "currency",
                        Options = new CurrencyOptions {
                            Precision = 0,
                            Symbol = "$"
                        }
                    },
                    new AirtableField { Name = "Recently-Sold-Last-Sold-Date",
                        Type = "dateTime",
                        Options = new DateTimeOptions
                        {
                            TimeZone = "utc",
                            DateFormat = new AirtableDateFormat
                            {
                                Format = "YYYY-MM-DD",
                                Name = "iso",
                            },
                            TimeFormat = new AirtableTimeFormat
                            {
                                Name = "24hour",
                                Format = "HH:mm"
                            },
                        }
                    },              
                    new AirtableField { Name = "Recently-Sold-Max-Price",
                        Type = "currency",
                        Options = new CurrencyOptions {
                            Precision = 0,
                            Symbol = "$"
                        }
                    },
                    new AirtableField { Name = "Recently-Sold-Min-Price",
                        Type = "currency",
                        Options = new CurrencyOptions {
                            Precision = 0,
                            Symbol = "$"
                        }
                    },

                    // Specializations
                    new AirtableField { Name = "Specialization-1-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Specialization-2-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Specialization-3-Name", Type = "singleLineText" },

                    // Marketing Area Cities
                    new AirtableField { Name = "Marketing-City-1-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Marketing-City-2-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Marketing-City-3-Name", Type = "singleLineText" },
                };


                // Prepare the Airtable API request
                var airtableRequest = new AirtableTableRequest
                {
                    Name = data.TableName,
                    Fields = fields
                };

                // JsonSerializerSettings to ensure camelCase serialization
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                // Serialize the airtableRequest object to JSON with camelCase
                var serializedJson = JsonConvert.SerializeObject(airtableRequest, jsonSettings);
                _logger.LogInformation("Serialized JSON being sent to Airtable: {SerializedJson}", serializedJson);

                // Initialize the RestClient for sending the HTTP request to the Airtable API
                var client = new RestClient(new RestClientOptions($"https://api.airtable.com/v0/meta/bases/{data.AirtableBaseId}/tables"));
                var request = new RestRequest("", Method.Post)
                    .AddHeader("Authorization", $"Bearer {data.AirtablePersonalToken}")  
                    .AddHeader("Content-Type", "application/json")                    
                    .AddParameter("application/json", serializedJson, ParameterType.RequestBody); 

                // Send the request to the Airtable API and await the response
                var response = await client.ExecuteAsync(request);

                // Check if the response was not successful
                if (!response.IsSuccessful)
                {
                    _logger.LogError("Airtable API error response: {ResponseContent}", response.Content);
                    return new StatusCodeResult((int)response.StatusCode); // Return the appropriate HTTP status code if there was an error
                }

                _logger.LogInformation("Airtable API response: {ResponseContent}", response.Content);

                // Return the successful response from Airtable as the function's response
                return new OkObjectResult(response.Content);
            }
            catch (Exception ex)
            {
                // Log any exception that occurs and return a 500 Internal Server Error response
                _logger.LogError(ex, "Error during table creation");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
