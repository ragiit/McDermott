namespace McHealthCare.Domain.Entities
{
    public partial class DoctorScheduleDetail : BaseAuditableEntity
    {
        public Guid DoctorScheduleId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public long Quota { get; set; } = 0;
        public bool UpdateToBpjs { get; set; } = false;

        public virtual DoctorSchedule? DoctorSchedule { get; set; }
    }
}