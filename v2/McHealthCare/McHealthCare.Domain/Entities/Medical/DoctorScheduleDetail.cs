namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DoctorScheduleDetail : BaseAuditableEntity
    {
        public Guid? DoctorScheduleId { get; set; }
        public string? Name { get; set; }
        public string? DayOfWeek { get; set; }
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public int? Quota { get; set; }
        public bool? UpdateToBpjs { get; set; }

        [SetToNull]
        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}