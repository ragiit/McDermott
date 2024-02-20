using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class KioskDepartement:BaseAuditableEntity
    {
        public int? ServiceKId { get; set; }
        public int? ServicePId { get; set; }

        public virtual Service? ServiceK { get; set; }
        public virtual Service? ServiceP { get; set; }
    }
}
