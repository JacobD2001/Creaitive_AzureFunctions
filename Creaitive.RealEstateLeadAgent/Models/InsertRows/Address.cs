using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Address
    {
        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("line")]
        public string? Line { get; set; }

        [JsonProperty("line2")]
        public string? Line2 { get; set; }

        [JsonProperty("postal_code")]
        public string? PostalCode { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("state_code")]
        public string? StateCode { get; set; }
    }
}
