namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class ConcoctionDto : IMapFrom<Concoction>
    {
        public Guid Id { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid? DrugDosageId { get; set; }
        public Guid? PrescribingDoctorId { get; set; }
        public Guid? MedicamentGroupId { get; set; }
        public Guid? DrugFormId { get; set; }
        public Guid? DrugRouteId { get; set; }
        public string? MedicamentName { get; set; }
        public string? MedicamenName { get; set; }

        public string? DrugFormName { get; set; }
        public string? DrugDosageName { get; set; }
        public long ConcoctionQty { get; set; } = 0;

        public DrugRouteDto? DrugRoute { get; set; }

        public DrugDosageDto? DrugDosage { get; set; }

        public DrugFormDto? DrugForm { get; set; }

        public PharmacyDto? Pharmacy { get; set; }

        public DoctorDto? PrescribingDoctor { get; set; }

        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}