namespace McHealthCare.Domain.Entities
{
    public class HealthCenter : BaseAuditableEntity
    {
        public Guid? CityId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? WebsiteLink { get; set; }

        public virtual City? City { get; set; }

        public virtual Province? Province { get; set; }
        public virtual Country? Country { get; set; }

        public virtual List<Building>? Buildings { get; set; }
    }
}