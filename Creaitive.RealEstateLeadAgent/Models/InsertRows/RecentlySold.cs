using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class RecentlySold
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("last_sold_date")]
        public string? LastSoldDate { get; set; }

        [JsonProperty("max")]
        public decimal Max { get; set; }

        [JsonProperty("min")]
        public decimal Min { get; set; }
    }
}
