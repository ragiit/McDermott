namespace McDermott.Application.Features.Commands.Config
{
    public class CountryCommand
    {
        #region GET

        public class GetCountryQuery : IRequest<(List<CountryDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Country, object>>> Includes { get; set; }
            public Expression<Func<Country, bool>> Predicate { get; set; }
            public Expression<Func<Country, Country>> Select { get; set; }

            public List<(Expression<Func<Country, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleCountryQuery : IRequest<CountryDto>
        {
            public List<Expression<Func<Country, object>>> Includes { get; set; }
            public Expression<Func<Country, bool>> Predicate { get; set; }
            public Expression<Func<Country, Country>> Select { get; set; }

            public List<(Expression<Func<Country, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateCountryQuery(List<CountryDto> CountrysToValidate) : IRequest<List<CountryDto>>
        {
            public List<CountryDto> CountrysToValidate { get; } = CountrysToValidate;
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

        public class CreateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>
        {
            public List<CountryDto> CountryDtos { get; set; } = CountryDtos;
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