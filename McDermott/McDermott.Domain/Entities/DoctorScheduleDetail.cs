using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class DoctorScheduleDetail : BaseAuditableEntity
    {
        public int DoctorScheduleId { get; set; }
        public int ServiceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public int Quota { get; set; } = 0;
        public bool UpdateToBpjs { get; set; } = false;

        public virtual Service? Service { get; set; }
        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}