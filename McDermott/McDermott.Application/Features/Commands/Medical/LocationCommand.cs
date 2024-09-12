namespace McDermott.Application.Features.Commands.Medical
{
    public class LocationCommand
    {
        #region GET 

        public class GetLocationQuery(Expression<Func<Location, bool>>? predicate = null, bool removeCache = false) : IRequest<List<LocationDto>>
        {
            public Expression<Func<Location, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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