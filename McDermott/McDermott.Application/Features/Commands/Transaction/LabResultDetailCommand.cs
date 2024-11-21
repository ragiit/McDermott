namespace McDermott.Application.Features.Commands.Transaction
{
    public class LabResultDetailCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetLabResultDetailQuery(Expression<Func<LabResultDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<LabResultDetailDto>>
        {
            public Expression<Func<LabResultDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateLabResultDetailRequest(LabResultDetailDto LabResultDetailDto) : IRequest<LabResultDetailDto>
        {
            public LabResultDetailDto LabResultDetailDto { get; set; } = LabResultDetailDto;
        }

        public class CreateListLabResultDetailRequest(List<LabResultDetailDto> GeneralConsultanCPPTDtos) : IRequest<List<LabResultDetailDto>>
        {
            public List<LabResultDetailDto> LabResultDetailDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateLabResultDetailRequest(LabResultDetailDto LabResultDetailDto) : IRequest<LabResultDetailDto>
        {
            public LabResultDetailDto LabResultDetailDto { get; set; } = LabResultDetailDto;
        }

        public class UpdateListLabResultDetailRequest(List<LabResultDetailDto> LabResultDetailDtos) : IRequest<List<LabResultDetailDto>>
        {
            public List<LabResultDetailDto> LabResultDetailDtos { get; set; } = LabResultDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteLabResultDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}