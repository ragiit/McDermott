using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class DoctorScheduleSlotDto : IMapFrom<DoctorScheduleSlot>
    {
        public Guid Id { get; set; }
        public Guid? DoctorScheduleId { get; set; }
        public Guid? PhysicianId { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }

        public TimeSpan? WorkTo { get; set; }

        public virtual DoctorScheduleDto? DoctorSchedule { get; set; }

        public virtual ApplicationUser? Physician { get; set; }
    }

    public class CreateUpdateDoctorScheduleSlotDto : IMapFrom<DoctorScheduleSlot>
    {
        public Guid Id { get; set; }
        public Guid? DoctorScheduleId { get; set; }
        public Guid? PhysicianId { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }

        public TimeSpan? WorkTo { get; set; }

        public virtual DoctorScheduleDto? DoctorSchedule { get; set; }

        public virtual ApplicationUser? Physician { get; set; }
    }
}