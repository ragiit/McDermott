using Mapster;
using McHealthCare.Application.Dtos.Inventory;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class BuildingLocationDto : IMapFrom<BuildingLocation>
    {
        public Guid Id { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? LocationId { get; set; }

        public virtual BuildingDto? Building { get; set; }
        public virtual LocationDto? Location { get; set; }
    }

    public class CreateUpdateBuildingLocationDto : IMapFrom<BuildingLocation>
    {
        public Guid Id { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? LocationId { get; set; }

        public virtual BuildingDto? Building { get; set; }
        public virtual LocationDto? Location { get; set; }
    }
}