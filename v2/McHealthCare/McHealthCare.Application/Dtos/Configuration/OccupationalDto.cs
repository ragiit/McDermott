using Mapster;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class OccupationalDto : IMapFrom<Occupational>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
    }

    public class CreateUpdateOccupationalDto : IMapFrom<Occupational>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
    }
}