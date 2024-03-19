namespace McDermott.Application.Features.Commands.Config
{
    public class DistrictCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetDistrictQuery(Expression<Func<District, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DistrictDto>>
        {
            public Expression<Func<District, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateDistrictRequest(DistrictDto DistrictDto) : IRequest<DistrictDto>
        {
            public DistrictDto DistrictDto { get; set; } = DistrictDto;
        }

        public class CreateListDistrictRequest(List<DistrictDto> GeneralConsultanCPPTDtos) : IRequest<List<DistrictDto>>
        {
            public List<DistrictDto> DistrictDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDistrictRequest(DistrictDto DistrictDto) : IRequest<DistrictDto>
        {
            public DistrictDto DistrictDto { get; set; } = DistrictDto;
        }

        public class UpdateListDistrictRequest(List<DistrictDto> DistrictDtos) : IRequest<List<DistrictDto>>
        {
            public List<DistrictDto> DistrictDtos { get; set; } = DistrictDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDistrictRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}