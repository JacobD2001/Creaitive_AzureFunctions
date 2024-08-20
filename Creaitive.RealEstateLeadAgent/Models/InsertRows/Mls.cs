using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Mls
    {
        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }

        [JsonProperty("license_number")]
        public string? LicenseNumber { get; set; }

        [JsonProperty("member")]
        public MlsMember? Member { get; set; }

        [JsonProperty("primary")]
        public bool? Primary { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
