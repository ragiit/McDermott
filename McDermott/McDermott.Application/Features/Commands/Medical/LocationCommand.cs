namespace McDermott.Application.Features.Commands.Medical
{
    public class LocationCommand
    {
        #region GET 

        public class GetLocationQuery(Expression<Func<Locations, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LocationDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateLocationQuery(Expression<Func<Locations, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
        }

        #endregion  

        #region CREATE

        public class CreateLocationRequest(LocationDto LocationDto) : IRequest<LocationDto>
        {
            public LocationDto LocationDto { get; set; } = LocationDto;
        }

        public class CreateListLocationRequest(List<LocationDto> LocationDtos) : IRequest<List<LocationDto>>
        {
            public List<LocationDto> LocationDtos { get; set; } = LocationDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateLocationRequest(LocationDto LocationDto) : IRequest<LocationDto>
        {
            public LocationDto LocationDto { get; set; } = LocationDto;
        }

        public class UpdateListLocationRequest(List<LocationDto> LocationDtos) : IRequest<List<LocationDto>>
        {
            public List<LocationDto> LocationDtos { get; set; } = LocationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteLocationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}