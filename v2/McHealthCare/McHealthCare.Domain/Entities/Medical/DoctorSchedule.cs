namespace McHealthCare.Domain.Entities.Medical{
    public partial class DoctorSchedule :BaseAuditableEntity{
        [StringLength(200)]
        public string? Name { get; set; } 

        public Guid? ServiceId { get; set; }

        // public List<Guid>? PhysicionIds { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }
    }
}