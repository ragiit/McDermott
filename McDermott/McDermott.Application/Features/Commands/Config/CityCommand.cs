namespace McDermott.Application.Features.Commands.Config
{
    public class CityCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetCityQuery(Expression<Func<City, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<CityDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<City, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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