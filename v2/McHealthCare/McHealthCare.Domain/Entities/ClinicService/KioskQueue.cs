using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Domain.Entities.ClinicService
{
    public class KioskQueue : BaseAuditableEntity
    {
        public Guid? KioskId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceKId { get; set; }
        public long? QueueNumber { get; set; }
        public string? QueueStage { get; set; }
        public string? QueueStatus { get; set; }
        public Guid? ClassTypeId { get; set; }

        [SetToNull]
        public virtual Kiosk? Kiosk { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }

        [SetToNull]
        public virtual Service? ServiceK { get; set; }

        [SetToNull]
        public virtual ClassType? ClassType { get; set; }
    }
}