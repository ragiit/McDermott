namespace McDermott.Application.Features.Commands
{
    public class CountryCommand
    {
        public class GetCountryQuery : IRequest<List<CountryDto>>;

        public class GetCountryByIdQuery : IRequest<CountryDto>
        {
            public int Id { get; set; }

            public GetCountryByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateCountryRequest : IRequest<CountryDto>
        {
            public CountryDto CountryDto { get; set; }

            public CreateCountryRequest(CountryDto countryDto)
            {
                CountryDto = countryDto;
            }
        }

        public class UpdateCountryRequest : IRequest<bool>
        {
            public CountryDto CountryDto { get; set; }

            public UpdateCountryRequest(CountryDto countryDto)
            {
                CountryDto = countryDto;
            }
        }

        public class DeleteCountryRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteCountryRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListCountryRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListCountryRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}