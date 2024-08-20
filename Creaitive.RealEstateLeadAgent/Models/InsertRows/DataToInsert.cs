using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class DataToInsert
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("full_name")]
        public string? FullName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("agent_rating")]
        public int? AgentRating { get; set; }

        [JsonProperty("last_updated")]
        public string? LastUpdated { get; set; }

        [JsonProperty("address")]
        public Address? Address { get; set; }

        [JsonProperty("phones")]
        public List<Phone>? Phones { get; set; }

        [JsonProperty("for_sale_price")]
        public ForSalePrice? ForSalePrice { get; set; }

        [JsonProperty("recently_sold")]
        public RecentlySold? RecentlySold { get; set; }

        [JsonProperty("agent_type")]
        public List<string>? AgentType { get; set; }

        [JsonProperty("specializations")]
        public List<Specialization>? Specializations { get; set; }

        [JsonProperty("marketing_area_cities")]
        public List<MarketingAreaCity>? MarketingAreaCities { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; } = "pending";
    }

}
