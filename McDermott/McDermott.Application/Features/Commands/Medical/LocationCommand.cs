namespace McDermott.Application.Features.Commands.Medical
{
    public class LocationCommand
    {
        public class GetLocationQuery : IRequest<List<LocationDto>>;

        public class GetLocationByIdQuery : IRequest<LocationDto>
        {
            public long Id { get; set; }

            public GetLocationByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateLocationRequest : IRequest<LocationDto>
        {
            public LocationDto LocationDto { get; set; }

            public CreateLocationRequest(LocationDto LocationDto)
            {
                this.LocationDto = LocationDto;
            }
        }

        public class UpdateLocationRequest : IRequest<bool>
        {
            public LocationDto LocationDto { get; set; }

            public UpdateLocationRequest(LocationDto LocationDto)
            {
                this.LocationDto = LocationDto;
            }
        }

        public class DeleteLocationRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteLocationRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListLocationRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListLocationRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}