using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class WellnessProgramAttendance : BaseAuditableEntity
    {
        public long WellnessProgramId { get; set; }
        public long PatientId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? Comments { get; set; }

        public User? Patient { get; set; }    
        public WellnessProgram? WellnessProgram { get; set; }
    }
}
