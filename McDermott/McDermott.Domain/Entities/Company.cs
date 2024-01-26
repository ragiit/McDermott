namespace McDermott.Domain.Entities
{
    public partial class Company : BaseAuditableEntity
    {
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }

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

        public int? CurrencyId { get; set; }
        public string? Logo { get; set; }

        public virtual City? City { get; set; }
        public virtual Province? Province { get; set; }
        public virtual Country? Country { get; set; }
    }
}