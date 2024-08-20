using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class ForSalePrice
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("last_listing_date")]
        public string? LastListingDate { get; set; }

        [JsonProperty("max")]
        public decimal Max { get; set; }

        [JsonProperty("min")]
        public decimal Min { get; set; }
    }
}
