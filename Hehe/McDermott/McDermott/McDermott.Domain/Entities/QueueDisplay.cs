namespace McDermott.Domain.Entities
{
    public class QueueDisplay : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<long>? CounterIds { get; set; }

        [SetToNull]
        public virtual List<Counter>? Counter { get; set; }
    }

    public class CreateUpdateQueueDisplayDto : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<long>? CounterIds { get; set; }
    }
}