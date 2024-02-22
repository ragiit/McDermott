using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultanCPPT : BaseAuditableEntity
    {
        public int GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;  

        public virtual GeneralConsultanService? GeneralConsultanService { get; set;}
    }
}
