namespace McHealthCare.Domain.Entities
{
    public class QueueDisplay : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<long>? CounterIds { get; set; }
        
        public virtual List<Counter>? Counter { get; set; }
    }
}