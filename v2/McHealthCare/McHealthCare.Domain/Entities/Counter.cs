namespace McHealthCare.Domain.Entities
{
    public partial class Counter : BaseAuditableEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceKId { get; set; }
        public Guid? PhysicianId { get; set; }
        public string? Status { get; set; }

        public virtual Service? Service { get; set; }

        public virtual Service? ServiceK { get; set; }

        // public virtual User? Physician { get; set; }
    }
}