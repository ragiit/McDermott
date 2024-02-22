using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Service : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Quota { get; set; } = string.Empty;
        public bool IsPatient {  get; set; }
        public bool IsKiosk { get; set; }
        //public int? ServicedId { get; set; }
        //public virtual Service? Serviced { get; set; }
    }
}