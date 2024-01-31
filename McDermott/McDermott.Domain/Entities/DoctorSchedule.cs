using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public List<int>? PhysicionIds { get; set; }
    }
}