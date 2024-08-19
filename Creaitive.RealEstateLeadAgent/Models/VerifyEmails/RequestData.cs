using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.VerifyEmails
{
    public class RequestData
    {
        [JsonProperty("emails")]
        public List<string>? Emails { get; set; }

        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "Default Task Name";
    }
}
