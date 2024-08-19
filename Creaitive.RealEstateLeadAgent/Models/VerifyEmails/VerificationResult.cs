using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.VerifyEmails
{
    public class VerificationResult
    {
        [JsonProperty("task_id")]
        public string? TaskId { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("count_checked")]
        public int CountChecked { get; set; }

        [JsonProperty("count_total")]
        public int CountTotal { get; set; }

        [JsonProperty("progress_percentage")]
        public float ProgressPercentage { get; set; }

        [JsonProperty("results")]
        public Dictionary<string, EmailVerificationResult>? Results { get; set; }
    }

    public class EmailVerificationResult
    {
        [JsonProperty("can_connect_smtp")]
        public bool CanConnectSmtp { get; set; }

        [JsonProperty("domain")]
        public string? Domain { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("has_inbox_full")]
        public bool HasInboxFull { get; set; }

        [JsonProperty("is_catch_all")]
        public bool IsCatchAll { get; set; }

        [JsonProperty("is_deliverable")]
        public bool IsDeliverable { get; set; }

        [JsonProperty("is_disabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("is_disposable")]
        public bool IsDisposable { get; set; }

        [JsonProperty("is_role_account")]
        public bool IsRoleAccount { get; set; }

        [JsonProperty("is_safe_to_send")]
        public bool IsSafeToSend { get; set; }

        [JsonProperty("is_spamtrap")]
        public bool IsSpamtrap { get; set; }

        [JsonProperty("is_valid_syntax")]
        public bool IsValidSyntax { get; set; }

        [JsonProperty("mx_accepts_mail")]
        public bool MxAcceptsMail { get; set; }

        [JsonProperty("mx_records")]
        public List<string>? MxRecords { get; set; }

        [JsonProperty("overall_score")]
        public string? OverallScore { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }
    }

}
