using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class DoctorScheduleDetailDto : IMapFrom<DoctorScheduleDetail>
    {
        public Guid Id { get; set; }
        public Guid? DoctorScheduleId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? DayOfWeek { get; set; }

        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public int? Quota { get; set; }
        public bool? UpdateToBpjs { get; set; }

        public virtual DoctorScheduleDto? DoctorSchedule { get; set; }
    }

    public class CreateUpdateDoctorScheduleDetailDto : IMapFrom<DoctorScheduleDetail>
    {
        public Guid Id { get; set; }
        public Guid? DoctorScheduleId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? DayOfWeek { get; set; }

        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public int? Quota { get; set; }
        public bool? UpdateToBpjs { get; set; }

        public virtual DoctorScheduleDto? DoctorSchedule { get; set; }
    }
}