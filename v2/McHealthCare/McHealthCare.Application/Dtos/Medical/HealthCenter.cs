using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class HealthCenterDto : IMapFrom<HealthCenter>
    {
        public Guid Id { get; set; }
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
        public virtual CityDto? City { get; set; }

        [SetToNull]
        public virtual ProvinceDto? Province { get; set; }

        [SetToNull]
        public virtual CountryDto? Country { get; set; }

        [SetToNull]
        public virtual List<BuildingDto>? Buildings { get; set; }
    }

    public class CreateUpdateHealthCenterDto : IMapFrom<HealthCenter>
    {
        public Guid Id { get; set; }
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
        public virtual CityDto? City { get; set; }

        [SetToNull]
        public virtual ProvinceDto? Province { get; set; }

        [SetToNull]
        public virtual CountryDto? Country { get; set; }

        [SetToNull]
        public virtual List<BuildingDto>? Buildings { get; set; }
    }
}