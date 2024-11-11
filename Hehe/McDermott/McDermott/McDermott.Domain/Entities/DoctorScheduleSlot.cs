namespace McDermott.Domain.Entities
{
    public partial class DoctorScheduleSlot : BaseAuditableEntity
    {
        public long? PhysicianId { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public long Quota { get; set; } = 0;
        public long ServiceId { get; set; }

        // Deprecated
        public long? DoctorScheduleId { get; set; }

        public virtual DoctorSchedule? DoctorSchedule { get; set; }

        public virtual User? Physician { get; set; }
        public virtual Service? Service { get; set; }
    }
}