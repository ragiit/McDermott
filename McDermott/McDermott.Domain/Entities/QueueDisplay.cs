namespace McDermott.Domain.Entities
{
    public class QueueDisplay : BaseAuditableEntity
    {
        public string? Name { get; set; }

        [SetToNull]
        public virtual List<Counter>? Counter { get; set; }
    }
}