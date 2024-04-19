namespace McDermott.Domain.Entities
{
    public partial class Company : BaseAuditableEntity
    {
        public long? CityId { get; set; }
        public long? ProvinceId { get; set; }
        public long? CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Email { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Website { get; set; }

        [StringLength(200)]
        public string? VAT { get; set; }

        public string? Street1 { get; set; }

        public string? Street2 { get; set; }

        [StringLength(200)]
        public string? Zip { get; set; }

        public long? CurrencyId { get; set; }
        public string? Logo { get; set; }

        [SetToNull]
        public virtual City? City { get; set; }

        [SetToNull]
        public virtual Province? Province { get; set; }

        [SetToNull]
        public virtual Country? Country { get; set; }

        [SetToNull]
        public virtual List<ReorderingRule>? ReorderingRules { get; set; }

        [SetToNull]
        public virtual List<Location>? Locations { get; set; }

    }
}