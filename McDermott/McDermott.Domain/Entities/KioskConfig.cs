using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class KioskConfig:BaseAuditableEntity
    {
        public string? Name { get; set; }
        public List<int>? ServiceIds { get; set; }

        //public virtual Service? Service { get; set; }
    }
}
