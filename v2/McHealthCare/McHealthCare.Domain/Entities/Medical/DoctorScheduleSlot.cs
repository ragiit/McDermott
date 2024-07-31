namespace McHealthCare.Domain.Entities.Medical{
    public partial class DoctorScheduleSlot :BaseAuditableEntity{
        public Guid? DoctorScheduleId { get; set; }
        public Guid? PhysicianId { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }

        public TimeSpan? WorkTo { get; set; }

        [SetToNull]
        public virtual DoctorSchedule? DoctorSchedule { get; set; }

        [SetToNull]
        public virtual ApplicationUser? Physician { get; set; }
    }
}