using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ProductCategory : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? CostingMethod { get; set; }
        public string? InventoryValuation { get; set; }

        public List<GeneralInformation>? GeneralInformation { get; set; }
    }
}