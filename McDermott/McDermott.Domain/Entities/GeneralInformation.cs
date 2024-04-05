using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralInformation :BaseAuditableEntity
    {
        public long? ProductId { get; set; }
        public long? BpjsClasificationId { get; set; }
        public long? UomId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public string? PurchaseUom { get; set; }
        public string? ProductType { get; set; }
        public string? InputType { get; set; }
        public string? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public string? Cost { get; set; }
        public string? InternalReference { get; set; }
     
        public Product? Product { get; set; }
        public Uom? Uom { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public Company? Company { get; set; }
    }
}
