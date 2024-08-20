using Google.Protobuf.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable
{
    public class AirtableField
    {
        public string? Name { get; set; }
        public string? Type { get; set; }

        [JsonIgnore]
        public FieldOptions? Options { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public FieldOptions? OptionsForSerialization
        {
            get
            {
                if (Type == "singleSelect" || Type == "multipleSelects")
                {
                    return Options;
                }
                return null;
            }
        }
    }


}
