namespace McDermott.Domain.Entities
{
    public class ProductCategory : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? CostingMethod { get; set; }
        public string? InventoryValuation { get; set; }
    }
}