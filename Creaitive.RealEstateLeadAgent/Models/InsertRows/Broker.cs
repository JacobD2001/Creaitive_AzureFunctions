using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Broker
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("accent_color")]
        public string? AccentColor { get; set; }
    }
}
