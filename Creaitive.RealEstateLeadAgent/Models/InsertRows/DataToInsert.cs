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

        [JsonProperty("background_photo")]
        public BackgroundPhoto? BackgroundPhoto { get; set; }

        [JsonProperty("broker")]
        public Broker? Broker { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("mls")]
        public List<Mls>? Mls { get; set; }

        [JsonProperty("mls_history")]
        public List<MlsHistory>? MlsHistory { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("web_url")]
        public string? WebUrl { get; set; }

        [JsonProperty("zips")]
        public List<string>? Zips { get; set; }

        [JsonProperty("office")]
        public Office? Office { get; set; }

        [JsonProperty("designations")]
        public List<Designation>? Designations { get; set; }

        [JsonProperty("served_areas")]
        public List<ServedArea>? ServedAreas { get; set; }

        [JsonProperty("slogan")]
        public string? Slogan { get; set; }
    }

}
