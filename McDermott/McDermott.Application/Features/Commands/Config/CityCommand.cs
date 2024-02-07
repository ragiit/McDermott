using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class CityCommand
    {
        public class GetCityQuery : IRequest<List<CityDto>>;

        public class GetCityByIdQuery : IRequest<CityDto>
        {
            public int Id { get; set; }

            public GetCityByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateCityRequest : IRequest<CityDto>
        {
            public CityDto CityDto { get; set; }

            public CreateCityRequest(CityDto CityDto)
            {
                this.CityDto = CityDto;
            }
        }

        public class UpdateCityRequest : IRequest<bool>
        {
            public CityDto CityDto { get; set; }

            public UpdateCityRequest(CityDto CityDto)
            {
                this.CityDto = CityDto;
            }
        }

        public class DeleteCityRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteCityRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListCityRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListCityRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}