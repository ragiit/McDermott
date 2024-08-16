using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class DoctorScheduleDto : IMapFrom<DoctorSchedule>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        public Guid? ServiceId { get; set; }

        // public List<Guid>? PhysicionIds { get; set; }

        public virtual ServiceDto? Service { get; set; }
    }

    public class CreateUpdateDoctorScheduleDto : IMapFrom<DoctorSchedule>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        public Guid? ServiceId { get; set; }

        // public List<Guid>? PhysicionIds { get; set; }

        public virtual ServiceDto? Service { get; set; }
    }
}