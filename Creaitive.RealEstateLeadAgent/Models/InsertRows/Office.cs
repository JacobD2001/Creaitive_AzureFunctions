using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class Office
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("address")]
        public Address? Address { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }

        [JsonProperty("phones")]
        public List<Phone>? Phones { get; set; }
    }
}
