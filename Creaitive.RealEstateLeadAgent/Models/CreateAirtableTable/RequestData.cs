using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.CreateAirtableTable
{
    public class RequestData
    {
        public string? AirtableBaseId { get; set; } //TODO: This model should derive from base model
        public string? AirtablePersonalToken { get; set; }
        public string? TableName { get; set; }
    }
}
