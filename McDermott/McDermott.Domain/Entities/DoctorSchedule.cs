namespace McDermott.Domain.Entities
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        public long PhysicionId { get; set; }

        public User? Physicion { get; set; }

        // Deprecated
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? ServiceId { get; set; }

        public List<long>? PhysicionIds { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }
    }
}