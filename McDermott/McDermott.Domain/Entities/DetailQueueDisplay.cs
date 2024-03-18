namespace McDermott.Domain.Entities
{
    public class DetailQueueDisplay : BaseAuditableEntity
    {
        public long? QueueDisplayId { get; set; }
        public long? CounterId { get; set; }

        [SetToNull]
        public virtual QueueDisplay? QueueDisplay { get; set; }

        [SetToNull]
        public virtual Counter? Counter { get; set; }
    }
}