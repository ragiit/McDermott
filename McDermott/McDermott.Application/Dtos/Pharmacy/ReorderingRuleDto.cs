namespace McDermott.Application.Dtos.Pharmacy
{
    public class ReorderingRuleDto : IMapFrom<ReorderingRule>
    {
        public long Id { get; set; }
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public float MinimumQuantity { get; set; } = 0;
        public float MaximumQuantity { get; set; } = 0;

        public LocationDto? Location { get; set; }
        public CompanyDto? Company { get; set; }
    }
}