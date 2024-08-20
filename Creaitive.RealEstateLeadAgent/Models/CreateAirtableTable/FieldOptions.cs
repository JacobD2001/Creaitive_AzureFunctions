using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable
{
    public class FieldOptionsBase { }

    public class SingleSelectOptions : FieldOptionsBase
    {
        public List<FieldChoice>? Choices { get; set; }
    }

    public class RatingOptions : FieldOptionsBase
    {
        public int? Max { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
    }

    public class DateTimeOptions : FieldOptionsBase
    {
        public AirtableDateFormat? DateFormat { get; set; }
        public AirtableTimeFormat? TimeFormat { get; set; }
        public string? TimeZone { get; set; }
    }

    public class AirtableDateFormat
    {
        public string? Format { get; set; }
        public string? Name { get; set; }
    }

    public class AirtableTimeFormat
    {
        public string? Name { get; set; }
        public string? Format { get; set; }
    }

    public class NumberOptions : FieldOptionsBase
    {
        public int? Precision { get; set; }
    }


}
