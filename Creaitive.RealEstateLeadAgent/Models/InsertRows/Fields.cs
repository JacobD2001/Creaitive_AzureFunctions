using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Fields
    {
        [JsonProperty("Agent-Email")]
        public string? Email { get; set; }

        [JsonProperty("E-mail-Content")]
        public string? EmailContent { get; set; }

        [JsonProperty("Status")]
        public string? Status { get; set; }

        [JsonProperty("Agent-First-Name")]
        public string? FirstName { get; set; }

        [JsonProperty("Agent-Last-Name")]
        public string? LastName { get; set; }

        [JsonProperty("Agent-Full-Name")]
        public string? FullName { get; set; }

        [JsonProperty("Agent-Description")]
        public string? Description { get; set; }

        [JsonProperty("Agent-Rating")]
        public int? AgentRating { get; set; }

        [JsonProperty("Agent-Last-Updated")]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("Agent-Address-City")]
        public string? AddressCity { get; set; }

        [JsonProperty("Agent-Address-Country")]
        public string? AddressCountry { get; set; }

        [JsonProperty("Agent-Address-Line")]
        public string? AddressLine { get; set; }

        [JsonProperty("Agent-Address-Postal-Code")]
        public string? AddressPostalCode { get; set; }

        [JsonProperty("Agent-Address-State-Code")]
        public string? AddressStateCode { get; set; }

        [JsonProperty("Agent-Phone-Office")]
        public string? PhoneOffice { get; set; }

        [JsonProperty("Agent-Phone-Mobile")]
        public string? PhoneMobile { get; set; }

        [JsonProperty("For-Sale-Count")]
        public int? ForSaleCount { get; set; }

        [JsonProperty("For-Sale-Max-Price")]
        public decimal? ForSaleMaxPrice { get; set; }

        [JsonProperty("For-Sale-Min-Price")]
        public decimal? ForSaleMinPrice { get; set; }

        //[JsonProperty("Recently-Sold-Count")]
        //public int? RecentlySoldCount { get; set; }

        [JsonProperty("Recently-Sold-Max-Price")]
        public decimal? RecentlySoldMaxPrice { get; set; }

        [JsonProperty("Recently-Sold-Min-Price")]
        public decimal? RecentlySoldMinPrice { get; set; }
    }

}
