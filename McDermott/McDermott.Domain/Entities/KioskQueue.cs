namespace McDermott.Domain.Entities
{
    public class KioskQueue : BaseAuditableEntity
    {
        public long? KioskId { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? QueueNumber { get; set; }
        public string? QueueStage { get; set; }
        public string? QueueStatus { get; set; }

        [SetToNull]
        public virtual Kiosk? Kiosk { get; set; }
        [SetToNull]
        public virtual Service? Service { get; set; }
        [SetToNull]
        public virtual Service? ServiceK { get; set; }
    }
}