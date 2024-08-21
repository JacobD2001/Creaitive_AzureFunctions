using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using Creaitive.RealEstateLeadAgent.Models.SendBulkEmails;
using Markdig;

namespace Creaitive.RealEstateLeadAgent.Functions
{
    public class SendBulkEmails
    {
        private readonly ILogger<SendBulkEmails> _logger;

        public SendBulkEmails(ILogger<SendBulkEmails> logger)
        {
            _logger = logger;
        }

        [Function("SendBulkEmails")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("SendBulkEmails function invoked.");

            try
            {
                // Parse request body to get baseId, tableIdOrName, and airtablePersonalToken
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(requestBody);
                string airtableBaseId = data.airtableBaseId;
                string tableIdOrName = data.tableIdOrName;
                string airtablePersonalToken = data.AirtablePersonalToken;

                _logger.LogInformation("Request received with baseId: {BaseId}, tableIdOrName: {TableIdOrName}", airtableBaseId, tableIdOrName);

                var emailDataList = await FetchEmailData(airtableBaseId, tableIdOrName, airtablePersonalToken);

                await SendBulkEmailsMethod(emailDataList);

                foreach (var emailData in emailDataList)
                {
                    await UpdateEmailStatus(airtableBaseId, tableIdOrName, emailData.RecordId, airtablePersonalToken);
                }

                return new OkObjectResult("Emails sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in sending bulk emails.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        private async Task<List<EmailData>> FetchEmailData(string baseId, string tableIdOrName, string airtablePersonalToken)
        {
            _logger.LogInformation("Fetching email data from Airtable.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {airtablePersonalToken}");
            var response = await httpClient.GetAsync($"https://api.airtable.com/v0/{baseId}/{tableIdOrName}?filterByFormula=Status='pending'");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                var emailDataList = new List<EmailData>();

                foreach (var record in jsonResponse.records)
                {
                    var emailContentMarkdown = record.fields["E-mail-Content"];

                    if (emailContentMarkdown != null)
                    {
                        var emailData = new EmailData
                        {
                            Email = record.fields["Agent-Email"],
                            Subject = record.fields["E-Mail-Subject"],
                            Content = ConvertMarkdownToHtml((string)emailContentMarkdown),
                            RecordId = record.id
                        };

                        emailDataList.Add(emailData);
                    }
                    else
                    {
                        _logger.LogWarning($"Email content is null for record {record.id}. Skipping this record.");
                    }
                }

                return emailDataList;
            }

            _logger.LogError($"Failed to fetch email data from Airtable. Status Code: {response.StatusCode}, Response: {responseContent}");
            throw new Exception("Failed to fetch email data from Airtable.");
        }


        private async Task SendBulkEmailsMethod(List<EmailData> emailDataList)
        {
            foreach (var emailData in emailDataList)
            {
                try
                {
                    if (string.IsNullOrEmpty(emailData.Content))
                    {
                        _logger.LogError($"Email content is null or empty for recipient {emailData.Email}. Skipping email.");
                        continue; // Skip sending the email if content is null or empty
                    }

                    var smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                    {
                        Credentials = new NetworkCredential("07cd807f8b11d9", "9d23e91ba76fc9"),
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("jehehaj291@segichen.com"),
                        Subject = emailData.Subject,
                        Body = ConvertMarkdownToHtml(emailData.Content), // Convert Markdown to HTML here
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(emailData.Email);

                    await smtpClient.SendMailAsync(mailMessage);

                    _logger.LogInformation($"Email sent to {emailData.Email}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to send email to {emailData.Email}: {ex.Message}");
                }
            }
        }



        private async Task UpdateEmailStatus(string baseId, string tableIdOrName, string recordId, string airtablePersonalToken)
        {
            _logger.LogInformation("Updating email status in Airtable for recordId: {RecordId}", recordId);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {airtablePersonalToken}");

            var updatePayload = new { fields = new { Status = "sent" } };
            var content = new StringContent(JsonConvert.SerializeObject(updatePayload), Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"https://api.airtable.com/v0/{baseId}/{tableIdOrName}/{recordId}", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Airtable Update Response: {ResponseContent}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to update status for record {recordId}: {responseContent}");
            }
        }

        private string ConvertMarkdownToHtml(string markdown)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdown, pipeline);
        }

    }
}
