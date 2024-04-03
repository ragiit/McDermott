using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ProductCategoryDto : IMapFrom<ProductCategory>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? CostingMethod { get; set; }
        public string? InventoryValuation { get; set; }
    }
}