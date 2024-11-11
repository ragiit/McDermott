namespace McDermott.Domain.Entities
{
    public partial class DoctorScheduleDetail : BaseAuditableEntity
    {
        public long DoctorScheduleId { get; set; }
        public long ServiceId { get; set; }

        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public long Quota { get; set; } = 0;
        public virtual Service? Service { get; set; }

        // Deprecated
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool UpdateToBpjs { get; set; } = false;

        [SetToNull]
        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}