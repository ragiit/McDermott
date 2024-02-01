using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class DoctorScheduleSlot : BaseAuditableEntity
    {
        public int DoctorScheduleId { get; set; }
        public DateTime StartDate { get; set; }

        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}