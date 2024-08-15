namespace McHealthCare.Application.Features.Commands.ClinicServices
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

        public class DeleteLabResultDetailRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}