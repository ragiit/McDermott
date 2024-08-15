namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class DrugDosageDto : IMapFrom<DrugDosage>
    {
        public Guid Id { get; set; }
        public Guid? DrugRouteId { get; set; }

        [Required]
        public string Frequency { get; set; } = string.Empty;

        public float TotalQtyPerDay { get; set; }
        public float Days { get; set; }

        public DrugRouteDto? DrugRoute { get; set; }
    }
}