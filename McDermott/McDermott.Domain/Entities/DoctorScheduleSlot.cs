namespace McDermott.Domain.Entities
{
    public partial class DoctorScheduleSlot : BaseAuditableEntity
    {
        public long DoctorScheduleId { get; set; }
        public long? PhysicianId { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public virtual DoctorSchedule? DoctorSchedule { get; set; }
        public virtual User? Physician { get; set; }
    }
}