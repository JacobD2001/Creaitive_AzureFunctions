using Creaitive.RealEstateLeadAgent.Models.VerifyEmails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Functions
{
    public class VerifyEmails
    {
        private readonly ILogger<VerifyEmails> _logger;

        public VerifyEmails(ILogger<VerifyEmails> logger)
        {
            _logger = logger;
        }

        [Function("VerifyEmails")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("VerifyEmails function invoked.");

            try
            {
                // Parse the request body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<RequestData>(requestBody);

                _logger.LogInformation("Received request with body: {Body}", requestBody);

                // Validate required fields
                if (data?.Emails == null || data.Emails.Count == 0 || string.IsNullOrEmpty(data.Key))
                {
                    _logger.LogError("Invalid input. Emails array and API key are required.");
                    return new BadRequestObjectResult(new { status = "error", message = "Invalid input. Emails array and API key are required." });
                }

                var createTaskUrl = "https://emailverifier.reoon.com/api/v1/create-bulk-verification-task/";

                var client = new RestClient(new RestClientOptions(createTaskUrl) { Timeout = TimeSpan.FromMilliseconds(30000) });
                var request = new RestRequest("", Method.Post)
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(new { name = data.Name, emails = data.Emails, key = data.Key });

                var taskCreationResponse = await client.ExecuteAsync(request);
                _logger.LogInformation("Task creation response: {Response}", taskCreationResponse.Content);

                var verificationResult = JsonConvert.DeserializeObject<VerificationResult>(taskCreationResponse.Content);

                // Extract and log the task ID
                var taskId = verificationResult?.TaskId;
                _logger.LogInformation("Task ID: {TaskId}", taskId);

                if (taskId == null)
                {
                    _logger.LogError("Task ID is null. Task creation failed.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                _logger.LogInformation("Task created successfully with ID: {TaskId}", taskId);

                var getResultUrl = $"https://emailverifier.reoon.com/api/v1/get-result-bulk-verification-task/?key={data.Key}&task_id={taskId}";

                async Task<VerificationResult> CheckTaskStatusAsync()
                {
                    var statusClient = new RestClient(getResultUrl);
                    var statusRequest = new RestRequest("", Method.Get);
                    return await Task.Run(async () =>
                    {
                        while (true)
                        {
                            var statusResponse = await statusClient.ExecuteAsync(statusRequest);
                            _logger.LogInformation("Raw status response: {Response}", statusResponse.Content);

                            var statusResult = JsonConvert.DeserializeObject<VerificationResult>(statusResponse.Content);

                            _logger.LogInformation("Current task status: {Status}", statusResult?.Status);

                            if (statusResult?.Status == "completed")
                            {
                                _logger.LogInformation("Final verification results: {Results}", JsonConvert.SerializeObject(statusResult));
                                return statusResult;
                            }
                            else if (statusResult?.Status == "waiting" || statusResult?.Status == "running")
                            {
                                await Task.Delay(10000); // Poll every 15 seconds
                            }
                            else
                            {
                                throw new Exception($"Task failed with status: {statusResult?.Status}");
                            }
                        }
                    });
                }

                var finalResult = await CheckTaskStatusAsync();

                _logger.LogInformation("Final result: {FinalResult}", JsonConvert.SerializeObject(finalResult));

                // Return the complete verification result directly
                return new OkObjectResult(finalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during email verification");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
