using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Counter:BaseAuditableEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? PhysicianId { get; set; }
        public string? Status { get; set; } 

        public virtual Service? Service { get; set; }
        public virtual Service? ServiceK { get; set; }
        public virtual User? Physician { get; set; }
    }
}
