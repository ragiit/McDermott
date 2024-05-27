namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionDto : IMapFrom<Concoction>
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? PrescribingDoctorId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? DrugFromId { get; set; }
        public long? UomId { get; set; }
        public string? MedicamentName { get; set; }
        public string? MedicamentGroupName { get; set; }
        public string? UomName { get; set; }
        public string? DrugFormName { get; set; }
        public string? DrugDosageName { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long Qty { get; set; } = 0;

        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public long QtyByDay { get; set; } = 0;

        public long? Days { get; set; }
        public long? TotalQty { get; set; }

        public UomDto? Uom { get; set; }
        public DrugDosageDto? DrugDosage { get; set; }
        public DrugFormDto? DrugForm { get; set; }
        public PharmacyDto? Pharmacy { get; set; }
        public UserDto? PrescribingDoctor { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}