using Creaitive.RealEstateLeadAgent.Models;
using Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                    new AirtableField { Name = "Status", Type = "singleSelect", Options = new FieldOptions { Choices = new List<string> { "pending", "sent" } }},
                    new AirtableField { Name = "Agent-First-Name", Type = "singleLineText" },
                    new AirtableField { Name = "Agent-Last-Name", Type = "singleLineText" },
                    // Add the remaining fields as required...
                };

                // Prepare the Airtable API request
                var airtableRequest = new AirtableTableRequest
                {
                    Name = data.TableName,
                    Fields = fields
                };

                var client = new RestClient(new RestClientOptions($"https://api.airtable.com/v0/meta/bases/{data.AirtableBaseId}/tables"));
                var request = new RestRequest("", Method.Post)
                    .AddHeader("Authorization", $"Bearer {data.AirtablePersonalToken}")
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(airtableRequest);

                // Make the API request
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    _logger.LogError("Airtable API error response: {ResponseContent}", response.Content);
                    return new StatusCodeResult((int)response.StatusCode);
                }

                _logger.LogInformation("Airtable API response: {ResponseContent}", response.Content);

                // Return the response from Airtable
                return new OkObjectResult(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during table creation");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
