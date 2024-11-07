using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleDto : IMapFrom<DoctorSchedule>
    {
        public long Id { get; set; }

        [Required]
        public long PhysicionId { get; set; }

        public User? Physicion { get; set; }

        // Deprecated
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? ServiceId { get; set; }

        public List<long>? PhysicionIds { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }

        //[StringLength(200)]
        //public string Name { get; set; } = string.Empty;

        //public long ServiceId { get; set; }

        //public List<long>? PhysicionIds { get; set; }
        //public string Physicions { get; set; } = string.Empty;

        //public ServiceDto? Service { get; set; }
    }

    public class CreateUpdateDoctorScheduleDto
    {
        public long Id { get; set; }

        public long PhysicionId { get; set; }

        public string Name { get; set; } = string.Empty;

        public long? ServiceId { get; set; }

        public List<long>? PhysicionIds { get; set; }
    }
}