namespace McDermott.Application.Dtos.Medical
{
    public class HealthCenterDto : IMapFrom<HealthCenter>
    {
        public long Id { get; set; }
        public long? CityId { get; set; }
        public long? ProvinceId { get; set; }
        public long? CountryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string? Type { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? WebsiteLink { get; set; }

        public CityDto? City { get; set; }
        public ProvinceDto? Province { get; set; }
        public CountryDto? Country { get; set; }
    }
}