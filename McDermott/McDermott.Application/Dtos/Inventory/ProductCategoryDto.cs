namespace McDermott.Application.Dtos.Inventory
{
    public class ProductCategoryDto : IMapFrom<ProductCategory>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public string? CostingMethod { get; set; }
        public string? InventoryValuation { get; set; }
    }
}