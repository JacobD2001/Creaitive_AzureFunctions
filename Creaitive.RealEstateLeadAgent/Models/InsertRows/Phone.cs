using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Phone
    {
        [JsonProperty("key")]
        public string? Key { get; set; }

        [JsonProperty("number")]
        public string? Number { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
