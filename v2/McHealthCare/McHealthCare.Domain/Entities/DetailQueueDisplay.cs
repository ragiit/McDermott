namespace McHealthCare.Domain.Entities
{
    public class DetailQueueDisplay : BaseAuditableEntity
    {
        public Guid? KioskQueueId { get; set; }
        public Guid? ServicekId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? NumberQueue { get; set; }

        
    }
}