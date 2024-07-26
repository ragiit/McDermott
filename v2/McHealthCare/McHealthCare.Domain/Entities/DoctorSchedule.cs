namespace McHealthCare.Domain.Entities
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public Guid ServiceId { get; set; }

        public List<long>? PhysicionIds { get; set; }

        
        public virtual Service? Service { get; set; }
    }
}