using Mapster;
using McHealthCare.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class CountryDto : IMapFrom<Country>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
         
    }

    public class CreateUpdateCountryDto : IMapFrom<Country>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}