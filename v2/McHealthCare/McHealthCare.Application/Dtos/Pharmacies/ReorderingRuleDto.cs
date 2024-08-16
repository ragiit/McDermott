namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class ReorderingRuleDto : IMapFrom<ReorderingRule>
    {
        public Guid Id { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? CompanyId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public float MinimumQuantity { get; set; } = 0;
        public float MaximumQuantity { get; set; } = 0;

        public LocationDto? Location { get; set; }
        public CompanyDto? Company { get; set; }
    }
}