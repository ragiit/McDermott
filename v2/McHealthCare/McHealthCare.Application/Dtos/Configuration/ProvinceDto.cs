using Mapster;
using McHealthCare.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class ProvinceDto : IMapFrom<ProvinceDto>
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public virtual CountryDto? Country { get; set; }
    }

    public class CreateUpdateProvinceDto : IMapFrom<ProvinceDto>
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}