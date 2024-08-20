using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class RequestData
    {
        [JsonProperty("tableIdOrName")]
        public string? TableIdOrName { get; set; }

        [JsonProperty("airtableBaseId")]
        public string? AirtableBaseId { get; set; }

        [JsonProperty("AirtablePersonalToken")]
        public string? AirtablePersonalToken { get; set; }

    }
}
