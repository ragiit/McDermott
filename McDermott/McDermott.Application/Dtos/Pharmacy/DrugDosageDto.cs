namespace McDermott.Application.Dtos.Pharmacy
{
    public class DrugDosageDto : IMapFrom<DrugDosage>
    {
        public long Id { get; set; }
        public long? DrugRouteId { get; set; }

        [Required]
        public string Frequency { get; set; } = string.Empty;

        public DrugRouteDto? DrugRoute { get; set; }
    }
}
