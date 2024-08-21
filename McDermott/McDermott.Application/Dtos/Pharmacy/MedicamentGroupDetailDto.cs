namespace McDermott.Application.Dtos.Pharmacy
{
    public class MedicamentGroupDetailDto : IMapFrom<MedicamentGroupDetail>
    {
        public long Id { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? MedicamentId { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public long? SignaId { get; set; }
        public long? FrequencyId { get; set; }
        public string FrequencyName { get; set; }
        public long? UnitOfDosageId { get; set; }
        public bool? AllowSubtitation { get; set; } = false;
        public string? MedicaneUnitDosage { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long? MedicaneDosage { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long Dosage { get; set; } = 0;

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long QtyByDay { get; set; } = 0;

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long? Days { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long? TotalQty { get; set; }

        public string? MedicaneName { get; set; }
        public string? Comment { get; set; }
        public string? ActiveComponentName { get; set; }

        public virtual ProductDto? Medicament { get; set; }
        public virtual DrugDosageDto? Frequency { get; set; }
        public virtual MedicamentGroupDto? MedicamentGroup { get; set; }
        public virtual UomDto? UnitOfDosage { get; set; }
        public virtual SignaDto? Signa { get; set; }
        public virtual List<ActiveComponentDto>? ActiveComponent { get; set; }
    }
}