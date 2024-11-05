using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramAttendanceDto : IMapFrom<WellnessProgramAttendance>
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long PatientId { get; set; }
        public DateTime AttendanceDate { get; set; } = DateTime.Now;
        public string? AttendanceStatus { get; set; }
        public string? Comments { get; set; }

        public UserDto? Patient { get; set; }
        public WellnessProgramDto? WellnessProgram { get; set; }

    }
          public class CreateUpdateWellnessProgramAttendanceDto  
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long PatientId { get; set; }
        public DateTime AttendanceDate { get; set; } = DateTime.Now;
        public string? AttendanceStatus { get; set; }
        public string? Comments { get; set; }
         
    }
}
