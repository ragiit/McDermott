namespace McDermott.Domain.Entities
{
    public partial class Kiosk : BaseAuditableEntity
    {
        public string? Type { get; set; }
        public string? NumberType { get; set; }
        public string? BPJS { get; set; }
        public bool? StageBpjs { get; set; }
        public int? PatientId { get; set; }
        public int? ServiceId { get; set; }
        public int? PhysicianId { get; set; }
        public virtual Service? Service { get; set; }
        public virtual User? Patient { get; set; }
        public virtual User? Physician { get; set; }
    }
}