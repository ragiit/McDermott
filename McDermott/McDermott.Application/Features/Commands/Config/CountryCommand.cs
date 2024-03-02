using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class CountryCommand
    {
        #region Get

        public class GetCountryQuery : IRequest<List<CountryDto>>;

        public class GetCountryByIdQuery(int id) : IRequest<CountryDto>
        {
            public int Id { get; set; } = id;
        }

        #endregion Get

        #region Create

        public class CreateCountryRequest : IRequest<CountryDto>
        {
            public CountryDto CountryDto { get; set; }

            public CreateCountryRequest(CountryDto countryDto)
            {
                CountryDto = countryDto;
            }
        }

        public class CreateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>
        {
            public List<CountryDto> CountryDtos { get; set; } = CountryDtos;
        }

        #endregion Create

        #region Update

        public class UpdateCountryRequest(CountryDto countryDto) : IRequest<bool>
        {
            public CountryDto CountryDto { get; set; } = countryDto;
        }

        #endregion Update

        #region Delete

        public class DeleteCountryRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteCountryRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListCountryRequest(List<int> id) : IRequest<bool>
        {
            public List<int> Id { get; set; } = id;
        }

        #endregion Delete
    }
}