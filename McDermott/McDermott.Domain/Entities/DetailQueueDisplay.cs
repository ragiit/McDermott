namespace McDermott.Domain.Entities
{
    public class DetailQueueDisplay : BaseAuditableEntity
    {
        public long? KioskQueueId { get; set; }
        public long? ServicekId { get; set; }
        public long? ServiceId { get; set; }
        public long? NumberQueue { get; set; }
    }
}