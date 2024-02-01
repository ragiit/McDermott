using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class DoctorScheduleSlotDto : IMapFrom<DoctorScheduleSlot>
    {
        public int Id { get; set; }
        public int DoctorScheduleId { get; set; }
        public DateTime StartDate { get; set; }

        public DoctorScheduleDto? DoctorSchedule { get; set; }
    }
}