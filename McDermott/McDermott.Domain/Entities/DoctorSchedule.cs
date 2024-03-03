namespace McDermott.Domain.Entities
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public int ServiceId { get; set; }

        public List<int>? PhysicionIds { get; set; }

        public virtual Service? Service { get; set; }
    }
}