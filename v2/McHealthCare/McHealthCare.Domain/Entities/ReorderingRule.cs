namespace McHealthCare.Domain.Entities
{
    public class ReorderingRule : BaseAuditableEntity
    {
        public Guid? LocationId { get; set; }
        public Guid? CompanyId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public float MinimumQuantity { get; set; } = 0;
        public float MaximumQuantity { get; set; } = 0;

        public virtual Location? Location { get; set; }

        public virtual Company? Company { get; set; }
    }
}