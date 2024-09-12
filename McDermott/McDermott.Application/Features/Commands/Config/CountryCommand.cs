namespace McDermott.Application.Features.Commands.Config
{
    public class CountryCommand
    {
        #region GET

        //public class GetCountryQuery(Expression<Func<Country, bool>>? predicate = null, bool removeCache = false) : IRequest<List<CountryDto>>
        //{
        //    public Expression<Func<Country, bool>> Predicate { get; } = predicate!;
        //    public bool RemoveCache { get; } = removeCache!;
        //}

        public class GetCountryQuery(Expression<Func<Country, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<CountryDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Country, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateCountryQuery(Expression<Func<Country, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Country, bool>> Predicate { get; } = predicate!;
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