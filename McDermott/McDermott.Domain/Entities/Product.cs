namespace McDermott.Domain.Entities
{
    public class Product : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public long? BpjsClassificationId { get; set; }
        public long? UomId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public long? PurchaseUomId { get; set; }
        public bool TraceAbility { get; set; } = false;
        public string? ProductType { get; set; }
        public string? HospitalType { get; set; }
        public string? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public string? Cost { get; set; }
        public string? InternalReference { get; set; }
        public string? Brand { get; set; }
        public string? EquipmentCode { get; set; }
        public DateTime? YearOfPurchase { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string? EquipmentCondition { get; set; }

        [SetToNull]
        public BpjsClassification? BpjsClassification { get; set; }

        [SetToNull]
        public Uom? Uom { get; set; }

        [SetToNull]
        public Uom? PurchaseUom { get; set; }

        [SetToNull]
        public ProductCategory? ProductCategory { get; set; }

        [SetToNull]
        public Company? Company { get; set; }

        [SetToNull]
        public List<Medicament>? Medicaments { get; set; }

        [SetToNull]
        public StockProduct? StockProduct { get; set; }

        [SetToNull]
        public List<ReceivingStockProduct>? ReceivingStockProduct { get; set; }
    }
}