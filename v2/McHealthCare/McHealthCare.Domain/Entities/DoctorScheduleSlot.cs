namespace McHealthCare.Domain.Entities
{
    public partial class DoctorScheduleSlot : BaseAuditableEntity
    {
        public Guid DoctorScheduleId { get; set; }
        public Guid? PhysicianId { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        
        public virtual DoctorSchedule? DoctorSchedule { get; set; }

        
        // public virtual User? Physician { get; set; }
    }
}