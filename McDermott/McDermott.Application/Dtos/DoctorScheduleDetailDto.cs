using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class DoctorScheduleDetailDto : IMapFrom<DoctorScheduleDetail>
    {
        public int Id { get; set; }
        public int DoctorScheduleId { get; set; }
        public int ServiceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        [StringLength(200)]
        public string WorkFrom { get; set; } = string.Empty;

        [StringLength(200)]
        public string WorkTo { get; set; } = string.Empty;

        public int Quota { get; set; } = 0;
        public bool UpdateToBpjs { get; set; } = false;

        public ServiceDto? Service { get; set; }
        public DoctorScheduleDto? DoctorSchedule { get; set; }
    }
}