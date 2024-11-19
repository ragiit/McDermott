namespace McDermott.Application.Dtos.Pharmacies
{
    public class DrugRouteDto : IMapFrom<DrugRoute>
    {
        public long Id { get; set; }

        [Required]
        public string Route { get; set; } = string.Empty;

        public string? Code { get; set; }
    }
}