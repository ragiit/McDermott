namespace McDermott.Domain.Entities
{
    public partial class Counter : BaseAuditableEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? PhysicianId { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }

        [SetToNull]
        public virtual Service? ServiceK { get; set; }

        [SetToNull]
        public virtual User? Physician { get; set; }
    }
}