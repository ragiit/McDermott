using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionDto : IMapFrom<Concoction>
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? PrescribingDoctorId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? DrugFormId { get; set; }
        public long? DrugRouteId { get; set; }
        public string? MedicamentName { get; set; }
        public string? MedicamenName { get; set; }

        public string? DrugFormName { get; set; }
        public string? DrugDosageName { get; set; }
        public long ConcoctionQty { get; set; } = 0;

        [SetToNull]
        public DrugRouteDto? DrugRoute { get; set; }

        [SetToNull]
        public DrugDosageDto? DrugDosage { get; set; }

        [SetToNull]
        public DrugFormDto? DrugForm { get; set; }

        [SetToNull]
        public PharmacyDto? Pharmacy { get; set; }

        [SetToNull]
        public UserDto? PrescribingDoctor { get; set; }

        [SetToNull]
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}