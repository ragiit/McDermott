namespace McDermott.Application.Features.Commands.Medical
{
    public class BuildingCommand
    {
        #region GET 

        public class GetBuildingQuery(Expression<Func<Building, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<BuildingDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Building, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateBuildingQuery(Expression<Func<Building, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Building, bool>> Predicate { get; } = predicate!;
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