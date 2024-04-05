namespace McDermott.Application.Features.Commands.Medical
{
    public class BuildingCommand
    {
        #region GET 

        public class GetBuildingQuery(Expression<Func<Building, bool>>? predicate = null, bool removeCache = false) : IRequest<List<BuildingDto>>
        {
            public Expression<Func<Building, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateBuildingRequest(BuildingDto BuildingDto) : IRequest<BuildingDto>
        {
            public BuildingDto BuildingDto { get; set; } = BuildingDto;
        }

        public class CreateListBuildingRequest(List<BuildingDto> BuildingDtos) : IRequest<List<BuildingDto>>
        {
            public List<BuildingDto> BuildingDtos { get; set; } = BuildingDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateBuildingRequest(BuildingDto BuildingDto) : IRequest<BuildingDto>
        {
            public BuildingDto BuildingDto { get; set; } = BuildingDto;
        }

        public class UpdateListBuildingRequest(List<BuildingDto> BuildingDtos) : IRequest<List<BuildingDto>>
        {
            public List<BuildingDto> BuildingDtos { get; set; } = BuildingDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteBuildingRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}