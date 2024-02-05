using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class DoctorScheduleDto : IMapFrom<DoctorSchedule>
    {
        public int Id { get; set; }
         
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public int ServiceId { get; set; }

        public List<int>? PhysicionIds { get; set; }
        public string Physicions { get; set; } = string.Empty;

        public ServiceDto? Service { get; set; }
    }
}