namespace McDermott.Application.Features.Commands.Config
{
    public class CountryCommand
    {
        #region GET

        public class GetCountryQuery(Expression<Func<Country, bool>>? predicate = null, bool removeCache = false) : IRequest<List<CountryDto>>
        {
            public Expression<Func<Country, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>
        {
            public CountryDto CountryDto { get; set; } = CountryDto;
        }

        public class CreateListCountryRequest(List<CountryDto> GeneralConsultanCPPTDtos) : IRequest<List<CountryDto>>
        {
            public List<CountryDto> CountryDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>
        {
            public CountryDto CountryDto { get; set; } = CountryDto;
        }

        public class UpdateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>
        {
            public List<CountryDto> CountryDtos { get; set; } = CountryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteCountryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}