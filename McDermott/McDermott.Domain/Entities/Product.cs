
namespace McDermott.Domain.Entities
{
    public class Product :BaseAuditableEntity
    {
        public string? Name { get; set; }
        public long? BpjsClassificationId { get; set; }
        public long? UomId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public long? PurchaseUomId { get; set; }
        public bool? TraceAbility { get; set; }
        public string? ProductType { get; set; }
        public string? HospitalType { get; set; }
        public string? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public string? Cost { get; set; }
        public string? InternalReference { get; set; }
      
        public BpjsClassification? BpjsClassification { get; set; }
        public Uom? Uom { get; set; }
        public Uom? PurchaseUom { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public Company? Company { get; set; }
        public List<Medicament>? Medicaments { get; set; }
        public StockProduct? StockProduct { get; set; }
    }
}
