using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class CountryCommand
    {
        #region Get
        public class GetCountryQuery : IRequest<List<CountryDto>>;

        public class GetCountryByIdQuery : IRequest<CountryDto>
        {
            public int Id { get; set; }

            public GetCountryByIdQuery(int id)
            {
                Id = id;
            }
        }

        #endregion

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
        #endregion

        #region Update
        public class UpdateCountryRequest : IRequest<bool>
        {
            public CountryDto CountryDto { get; set; }

            public UpdateCountryRequest(CountryDto countryDto)
            {
                CountryDto = countryDto;
            }
        }
        #endregion

        #region Delete
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
                Id = id;
            }
        }
        #endregion 
    }
}