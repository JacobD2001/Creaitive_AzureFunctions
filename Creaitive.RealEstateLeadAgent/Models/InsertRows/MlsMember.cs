using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.InsertRows
{
    public class MlsMember
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
    }
}
