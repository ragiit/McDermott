namespace McDermott.Application.Features.Commands.Config
{
    public class CityCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleCityQuery : IRequest<CityDto>
        {
            public List<Expression<Func<City, object>>> Includes { get; set; }
            public Expression<Func<City, bool>> Predicate { get; set; }
            public Expression<Func<City, City>> Select { get; set; }

            public List<(Expression<Func<City, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetCityQuery : IRequest<(List<CityDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<City, object>>> Includes { get; set; }
            public Expression<Func<City, bool>> Predicate { get; set; }
            public Expression<Func<City, City>> Select { get; set; }

            public List<(Expression<Func<City, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateCityQuery(List<CityDto> CitysToValidate) : IRequest<List<CityDto>>
        {
            public List<CityDto> CitysToValidate { get; } = CitysToValidate;
        }

        public class ValidateCityQuery(Expression<Func<City, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<City, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateCityRequest(CityDto CityDto) : IRequest<CityDto>
        {
            public CityDto CityDto { get; set; } = CityDto;
        }

        public class CreateListCityRequest(List<CityDto> GeneralConsultanCPPTDtos) : IRequest<List<CityDto>>
        {
            public List<CityDto> CityDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateCityRequest(CityDto CityDto) : IRequest<CityDto>
        {
            public CityDto CityDto { get; set; } = CityDto;
        }

        public class UpdateListCityRequest(List<CityDto> CityDtos) : IRequest<List<CityDto>>
        {
            public List<CityDto> CityDtos { get; set; } = CityDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteCityRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}