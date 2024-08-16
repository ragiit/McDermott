namespace McHealthCare.Application.Dtos.Inventory
{
    public class ProductDto : IMapFrom<Product>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; }

        public Guid? BpjsClassificationId { get; set; }
        public Guid? UomId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? PurchaseUomId { get; set; }
        public bool TraceAbility { get; set; } = false;
        public string? ProductType { get; set; }
        public string? HospitalType { get; set; }
        public long? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public long? Cost { get; set; }
        public bool IsTopicalMedication { get; set; }
        public bool IsOralMedication { get; set; }
        public string? InternalReference { get; set; }
        public string? UomName { get; set; }
        public long? Qtys { get; set; }
        public string? Brand { get; set; }
        public string? EquipmentCode { get; set; }
        public DateTime? YearOfPurchase { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string? EquipmentCondition { get; set; }

        public virtual UomDto? Uom { get; set; }

        public virtual UomDto? PurchaseUom { get; set; }

        public virtual ProductCategoryDto? ProductCategory { get; set; }

        public virtual CompanyDto? Company { get; set; }

        public virtual StockProductDto? StockProducts { get; set; }
    }

    public class ProductDetailDto
    {
        public Guid Id { get; set; }
        public Guid? MedicamentId { get; set; }

        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? FrequencyId { get; set; }
        public Guid? RouteId { get; set; }
        public Guid UomId { get; set; }
        public Guid? UomMId { get; set; }
        public List<Guid>? ActiveComponentId { get; set; }
        public bool TraceAbility { get; set; } = false;
        public bool? PregnancyWarning { get; set; } = false;
        public bool? Cronies { get; set; }
        public bool? Pharmacologi { get; set; } = false;
        public bool? Weather { get; set; } = false;
        public bool? Food { get; set; } = false;
        public string? MontlyMax { get; set; }
        public Guid? FormId { get; set; }
        public long Dosage { get; set; } = 0;
        public string? UomName { get; set; }
        public long? Qtys { get; set; }
        public Guid? BpjsClassificationId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid PurchaseUomId { get; set; }
        public string? ProductType { get; set; }
        public string? HospitalType { get; set; }
        public long? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public long? Cost { get; set; }
        public string? InternalReference { get; set; }
        public string? Brand { get; set; }
        public string? EquipmentCode { get; set; }
        public DateTime? YearOfPurchase { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string? EquipmentCondition { get; set; }
        public bool IsTopicalMedication { get; set; }
        public bool IsOralMedication { get; set; }
    }
}