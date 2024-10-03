namespace McDermott.Application.Features.Commands.Medical
{
    public class BuildingLocationCommand
    {
        #region GET

        public class GetBuildingLocationQuery(Expression<Func<BuildingLocation, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<BuildingLocation, object>>>? includes = null, Expression<Func<BuildingLocation, BuildingLocation>>? select = null) : IRequest<(List<BuildingLocationDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<BuildingLocation, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<BuildingLocation, object>>> Includes { get; } = includes!;
            public Expression<Func<BuildingLocation, BuildingLocation>>? Select { get; } = select!;
        }

        public class ValidateBuildingLocationQuery(Expression<Func<BuildingLocation, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<BuildingLocation, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET



        #region CREATE

        public class CreateBuildingLocationRequest(BuildingLocationDto BuildingLocationDto) : IRequest<BuildingLocationDto>
        {
            public BuildingLocationDto BuildingLocationDto { get; set; } = BuildingLocationDto;
        }

        public class CreateListBuildingLocationRequest(List<BuildingLocationDto> BuildingLocationDtos) : IRequest<List<BuildingLocationDto>>
        {
            public List<BuildingLocationDto> BuildingLocationDtos { get; set; } = BuildingLocationDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateBuildingLocationRequest(BuildingLocationDto BuildingLocationDto) : IRequest<BuildingLocationDto>
        {
            public BuildingLocationDto BuildingLocationDto { get; set; } = BuildingLocationDto;
        }

        public class UpdateListBuildingLocationRequest(List<BuildingLocationDto> BuildingLocationDtos) : IRequest<List<BuildingLocationDto>>
        {
            public List<BuildingLocationDto> BuildingLocationDtos { get; set; } = BuildingLocationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteBuildingLocationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}