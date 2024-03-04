namespace McDermott.Domain.Entities
{
    public class KioskQueue : BaseAuditableEntity
    {
        public int? KioskId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? NoQueue { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public virtual Kiosk? Kiosk { get; set; }
        [SetToNull]
        public virtual Service? Service { get; set; }
        [SetToNull]
        public virtual Service? ServiceK { get; set; }
    }
}