namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Location : BaseAuditableEntity
    {
        public Guid? CityId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? CountryId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? WebsiteLink { get; set; }

        [SetToNull]
        public virtual City? City { get; set; }

        [SetToNull]
        public virtual Province? Province { get; set; }

        [SetToNull]
        public virtual Country? Country { get; set; }

        [SetToNull]
        public virtual List<Building>? Buildings { get; set; }
    }
}