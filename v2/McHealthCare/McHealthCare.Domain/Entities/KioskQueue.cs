namespace McHealthCare.Domain.Entities
{
    public class KioskQueue : BaseAuditableEntity
    {
        public Guid? KioskId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceKId { get; set; }
        public Guid? QueueNumber { get; set; }
        public string? QueueStage { get; set; }
        public string? QueueStatus { get; set; }
        public Guid? ClassTypeId { get; set; }

        
        public virtual Kiosk? Kiosk { get; set; }

        
        public virtual Service? Service { get; set; }

        
        public virtual Service? ServiceK { get; set; }

        
        public virtual ClassType? ClassType { get; set; }
    }
}