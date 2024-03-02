namespace McDermott.Application.Features.Commands.Medical
{
    public class LocationCommand
    {
        public class GetLocationQuery : IRequest<List<LocationDto>>;

        public class GetLocationByIdQuery : IRequest<LocationDto>
        {
            public int Id { get; set; }

            public GetLocationByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteLocationRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListLocationRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListLocationRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}