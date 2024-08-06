using Mapster;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class ReligionDto : IMapFrom<Religion>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; 
    }

    public class CreateUpdateReligionDto : IMapFrom<Religion>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; 
    }
}