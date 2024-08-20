using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable
{
    public class AirtableTableRequest
    {
        public string? Name { get; set; }
        public List<AirtableField>? Fields { get; set; }
    }
}
