using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaitive.RealEstateLeadAgent.Models.SendBulkEmails
{
    public class EmailData
    {
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? RecordId { get; set; }  
    }

}
