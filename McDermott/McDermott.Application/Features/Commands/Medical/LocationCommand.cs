namespace McDermott.Application.Features.Commands.Medical
{
    public class LocationCommand
    {
        #region GET

        public class GetSingleLocationQuery : IRequest<LocationDto>
        {
            public List<Expression<Func<Locations, object>>> Includes { get; set; }
            public Expression<Func<Locations, bool>> Predicate { get; set; }
            public Expression<Func<Locations, Locations>> Select { get; set; }

            public List<(Expression<Func<Locations, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetLocationQuery : IRequest<(List<LocationDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Locations, object>>> Includes { get; set; }
            public Expression<Func<Locations, bool>> Predicate { get; set; }
            public Expression<Func<Locations, Locations>> Select { get; set; }

            public List<(Expression<Func<Locations, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateLocationQuery(Expression<Func<Locations, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateLocationsQuery(List<LocationDto> LocationssToValidate) : IRequest<List<LocationDto>>
        {
            public List<LocationDto> LocationssToValidate { get; } = LocationssToValidate;
        }

        #endregion GET

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