namespace McHealthCare.Application.Dtos.Configuration
{
    public class CompanyDto : IMapFrom<Company>
    {
        public Guid Id { get; set; }
        public Guid? CityId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? CountryId { get; set; }

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

        public long? CurrencyId { get; set; } // skip
        public string? Logo { get; set; }

        public virtual CityDto? City { get; set; }
        public virtual ProvinceDto? Province { get; set; }
        public virtual CountryDto? Country { get; set; }
    }

    public class CreateUpdateCompanyDto
    {
        public Guid Id { get; set; }
        public Guid? CityId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? CountryId { get; set; }

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
    }
}