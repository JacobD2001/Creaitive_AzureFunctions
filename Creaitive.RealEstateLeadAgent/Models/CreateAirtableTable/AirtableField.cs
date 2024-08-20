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
                // Return options only for the relevant types
                if (Type == "singleSelect" || Type == "multipleSelects" || Type == "rating" || Type == "dateTime" || Type == "number" || Type == "currency")
                {
                    return Options;
                }
                return null;
            }
        }
    }
}
