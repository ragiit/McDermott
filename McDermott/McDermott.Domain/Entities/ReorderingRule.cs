namespace McDermott.Domain.Entities
{
    public class ReorderingRule : BaseAuditableEntity
    {
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public float MinimumQuantity { get; set; } = 0;
        public float MaximumQuantity { get; set; } = 0;

        [SetToNull]
        public virtual Locations? Location { get; set; }

        [SetToNull]
        public virtual Company? Company { get; set; }
    }
}