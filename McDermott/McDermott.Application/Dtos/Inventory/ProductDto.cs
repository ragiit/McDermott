﻿using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class ProductDto : IMapFrom<Product>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; }

        public long? BpjsClassificationId { get; set; }
        public long? UomId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public long? PurchaseUomId { get; set; }
        public bool TraceAbility { get; set; } = false;
        public string? ProductType { get; set; }
        public string? HospitalType { get; set; }
        public long? SalesPrice { get; set; }
        public string? Tax { get; set; }
        public long? Cost { get; set; }
        public bool IsTopicalMedication { get; set; } = false;
        public bool IsOralMedication { get; set; } = false;
        public string? InternalReference { get; set; }
        public string? UomName { get; set; }
        public long? Qtys { get; set; }
        public string? Brand { get; set; }
        public string? EquipmentCode { get; set; }
        public DateTime? YearOfPurchase { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public string? EquipmentCondition { get; set; }

        [SetToNull]
        public virtual UomDto? Uom { get; set; }

        [SetToNull]
        public virtual UomDto? PurchaseUom { get; set; }

        [SetToNull]
        public virtual ProductCategoryDto? ProductCategory { get; set; }

        [SetToNull]
        public virtual CompanyDto? Company { get; set; }

        [SetToNull]
        public virtual StockProductDto? StockProducts { get; set; }
    }

    public class ProductDetailDto
    {
        public long Id { get; set; }
        public long? MedicamentId { get; set; }

        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; }

        public long? ProductId { get; set; }
        public long? FrequencyId { get; set; }
        public long? RouteId { get; set; }
        public long? UomId { get; set; }
        public long? UomMId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public bool TraceAbility { get; set; } = false;
        public bool? PregnancyWarning { get; set; } = false;
        public bool? Cronies { get; set; }
        public bool? Pharmacologi { get; set; } = false;
        public bool? Weather { get; set; } = false;
        public bool? Food { get; set; } = false;
        public string? MontlyMax { get; set; }
        public long? FormId { get; set; }
        public long Dosage { get; set; } = 0;
        public string? UomName { get; set; }
        public long? Qtys { get; set; }
        public long? BpjsClassificationId { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? CompanyId { get; set; }
        public long? PurchaseUomId { get; set; }
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
        public bool IsTopicalMedication { get; set; } = false;
        public bool IsOralMedication { get; set; } = false;
        public bool HighAlert { get; set; } = false;
    }
}