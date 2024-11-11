namespace McDermott.Domain.Entities
{
    public partial class Kiosk : BaseAuditableEntity
    {
        public string? Type { get; set; }
        public string? NumberType { get; set; }
        public string? BPJS { get; set; }
        public bool? StageBpjs { get; set; }
        public long? PatientId { get; set; }
        public long? ServiceId { get; set; }
        public long? PhysicianId { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }

        [SetToNull]
        public virtual User? Patient { get; set; }

        [SetToNull]
        public virtual User? Physician { get; set; }
    }
}