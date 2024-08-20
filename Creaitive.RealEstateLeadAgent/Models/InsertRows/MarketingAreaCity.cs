using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class MarketingAreaCity
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("state_code")]
        public string? StateCode { get; set; }
    }
}
