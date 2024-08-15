using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Domain.Entities.ClinicService
{
    public partial class Kiosk : BaseAuditableEntity
    {
        public Guid? PatientId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? PhysicianId { get; set; }
        public string? Type { get; set; }
        public string? NumberType { get; set; }
        public string? BPJS { get; set; }
        public bool? StageBpjs { get; set; }


        public virtual Service? Service { get; set; }


        public virtual Patient? Patient { get; set; }
        public virtual Doctor? Physician { get; set; }
    }
}