namespace McDermott.Application.Features.Commands.Config
{
    public class ProvinceCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetProvinceQuery(Expression<Func<Province, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProvinceDto>>
        {
            public Expression<Func<Province, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class CreateListProvinceRequest(List<ProvinceDto> GeneralConsultanCPPTDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> ProvinceDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class UpdateListProvinceRequest(List<ProvinceDto> ProvinceDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> ProvinceDtos { get; set; } = ProvinceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProvinceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}