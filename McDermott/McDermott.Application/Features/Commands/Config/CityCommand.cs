namespace McDermott.Application.Features.Commands.Config
{
    public class CityCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetCityQuery(Expression<Func<City, bool>>? predicate = null, bool removeCache = false) : IRequest<List<CityDto>>
        {
            public Expression<Func<City, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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