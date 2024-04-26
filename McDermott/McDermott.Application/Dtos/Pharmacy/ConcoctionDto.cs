
namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionDto
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? PrescribingDoctorId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? DrugFromId { get; set; }
        public long? UomId { get; set; }
        public string? MedicamentName { get; set; }
        public float QtyDose { get; set; }
        public float QtyPerDay { get; set; }
        public float Days { get; set; }
        public float TotalQty { get; set; }

        public UomDto? Uom { get; set; }
        public DrugDosageDto? DrugDosage { get; set; }
        public DrugFormDto? DrugForm { get; set; }
        public PharmacyDto? Pharmacy { get; set; }
        public UserDto? PrescribingDoctor { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}
