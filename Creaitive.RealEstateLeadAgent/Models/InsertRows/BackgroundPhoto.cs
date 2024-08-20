using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class BackgroundPhoto
    {
        [JsonProperty("href")]
        public string? Href { get; set; }
    }
}
