namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DoctorScheduleDetail : BaseAuditableEntity
    {
        public Guid? DoctorScheduleId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? DayOfWeek { get; set; }

        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public int? Quota { get; set; }
        public bool? UpdateToBpjs { get; set; }

        [SetToNull]
        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}