namespace McDermott.Domain.Entities
{
    public class KioskQueue : BaseAuditableEntity
    {
        public long? KioskId { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? NoQueue { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public virtual Kiosk? Kiosk { get; set; }
        [SetToNull]
        public virtual Service? Service { get; set; }
        [SetToNull]
        public virtual Service? ServiceK { get; set; }
    }
}