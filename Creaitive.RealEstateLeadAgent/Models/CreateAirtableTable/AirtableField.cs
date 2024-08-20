using Newtonsoft.Json;

namespace Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable
{
    public class AirtableField
    {
        public string? Name { get; set; }
        public string? Type { get; set; }

        [JsonIgnore]
        public FieldOptionsBase? Options { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public FieldOptionsBase? OptionsForSerialization
        {
            get
            {
                // Ensure options are included only for specific types
                if (Type == "singleSelect" || Type == "multipleSelects" || Type == "rating")
                {
                    return Options;
                }
                return null;
            }
        }
    }
}
