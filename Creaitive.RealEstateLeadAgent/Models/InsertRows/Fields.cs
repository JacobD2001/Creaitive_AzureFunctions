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

        [JsonProperty("Agent-Address-Line")]
        public string? AddressLine { get; set; }

        [JsonProperty("Agent-Address-Postal-Code")]
        public string? AddressPostalCode { get; set; }

        [JsonProperty("Agent-Address-State")]
        public string? AddressState { get; set; }

        [JsonProperty("Agent-Phone-Mobile")]
        public string? PhoneMobile { get; set; }

        [JsonProperty("For-Sale-Count")]
        public int? ForSaleCount { get; set; }

        [JsonProperty("For-Sale-Max-Price")]
        public decimal? ForSaleMaxPrice { get; set; }

        [JsonProperty("For-Sale-Min-Price")]
        public decimal? ForSaleMinPrice { get; set; }

        [JsonProperty("For-Sale-Last-Listing-Date")]
        public DateTime? ForSaleLastListingDate { get; set; }

        [JsonProperty("Recently-Sold-Max-Price")]
        public decimal? RecentlySoldMaxPrice { get; set; }

        [JsonProperty("Recently-Sold-Min-Price")]
        public decimal? RecentlySoldMinPrice { get; set; }

        [JsonProperty("Recently-Sold-Last-Sold-Date")]
        public DateTime? RecentlySoldLastSoldDate { get; set; }

        [JsonProperty("Agent-Role")]
        public string? Role { get; set; }

        [JsonProperty("Agent-Title")]
        public string? Title { get; set; }

        [JsonProperty("Agent-Website")]
        public string? Website { get; set; }

        [JsonProperty("Office-Name")]
        public string? OfficeName { get; set; }

        [JsonProperty("Office-Slogan")]
        public string? OfficeSlogan { get; set; }

        [JsonProperty("Office-Website")]
        public string? OfficeWebsite { get; set; }

        [JsonProperty("Office-Email")]
        public string? OfficeEmail { get; set; }

        [JsonProperty("Office-Address-City")]
        public string? OfficeAddressCity { get; set; }

        [JsonProperty("Office-Address-Line")]
        public string? OfficeAddressLine { get; set; }

        [JsonProperty("Office-Address-Postal-Code")]
        public string? OfficeAddressPostalCode { get; set; }

        [JsonProperty("Office-Address-State")]
        public string? OfficeAddressState { get; set; }

        [JsonProperty("Office-Phone")]
        public string? OfficePhone { get; set; }

        [JsonProperty("Specialization-1-Name")]
        public string? Specialization1Name { get; set; }

        [JsonProperty("Specialization-2-Name")]
        public string? Specialization2Name { get; set; }

        [JsonProperty("Specialization-3-Name")]
        public string? Specialization3Name { get; set; }

        [JsonProperty("Marketing-City-1-Name")]
        public string? MarketingCity1Name { get; set; }

        [JsonProperty("Marketing-City-2-Name")]
        public string? MarketingCity2Name { get; set; }

        [JsonProperty("Marketing-City-3-Name")]
        public string? MarketingCity3Name { get; set; }
    }

}
