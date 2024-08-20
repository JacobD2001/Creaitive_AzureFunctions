using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class MlsHistory
    {
        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }

        [JsonProperty("inactivation_date")]
        public string? InactivationDate { get; set; }

        [JsonProperty("member")]
        public MlsMember? Member { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
