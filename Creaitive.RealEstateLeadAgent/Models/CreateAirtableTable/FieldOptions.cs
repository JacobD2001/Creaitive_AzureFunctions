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

}
