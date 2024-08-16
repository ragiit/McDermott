namespace McHealthCare.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public Guid? BpjsClassificationId { get; set; }
        public Guid? UomId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? PurchaseUomId { get; set; }
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
        public bool IsTopicalMedication { get; set; }
        public bool IsOralMedication { get; set; }

        public BpjsClassification? BpjsClassification { get; set; }

        public Uom? Uom { get; set; }

        public Uom? PurchaseUom { get; set; }

        public ProductCategory? ProductCategory { get; set; }

        public Company? Company { get; set; }

        public List<Medicament>? Medicaments { get; set; }

        public StockProduct? StockProduct { get; set; }

        public List<ReceivingStockProduct>? ReceivingStockProduct { get; set; }
    }
}