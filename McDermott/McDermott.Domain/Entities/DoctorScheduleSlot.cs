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
        public int? PhysicianId { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan WorkFrom { get; set; }  

        public TimeSpan WorkTo { get; set; }  

        public virtual DoctorSchedule? DoctorSchedule { get; set; }
        public virtual User? Physician { get; set; }
    }
}