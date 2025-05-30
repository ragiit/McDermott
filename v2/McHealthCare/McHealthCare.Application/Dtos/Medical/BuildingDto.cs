using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class BuildingDto : IMapFrom<Building>
    {
        public Guid Id { get; set; }
        public Guid? HealthCenterId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Name not null!!")]
        public string? Name { get; set; }
        [StringLength(200)]
        public string? Code { get; set; }

        public virtual HealthCenterDto? HealthCenter { get; set; } 
        public virtual List<BuildingLocationDto>? BuildingLocations { get; set; }
    }

    public class CreateUpdateBuildingDto : IMapFrom<Building>
    {
        public Guid Id { get; set; }
        public Guid? HealthCenterId { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage ="Name not null!!")]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Code { get; set; }

        public virtual HealthCenterDto? HealthCenter { get; set; }

        public virtual List<BuildingLocationDto>? BuildingLocations { get; set; }
    }
}