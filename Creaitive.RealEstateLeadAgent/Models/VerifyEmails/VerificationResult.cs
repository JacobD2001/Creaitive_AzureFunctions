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

    }
}
