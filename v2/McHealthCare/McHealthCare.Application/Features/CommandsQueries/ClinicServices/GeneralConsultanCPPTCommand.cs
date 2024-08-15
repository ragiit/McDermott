namespace McHealthCare.Application.Features.Commands.ClinicServices
{
    public class GeneralConsultanCPPTCommand
    {
        #region Get

        public class GetGeneralConsultanCPPTQuery(Expression<Func<GeneralConsultanCPPT, bool>>? predicate = null, bool RemoveCache = false) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = RemoveCache!;
        }

        #endregion Get

        #region Create

        public class CreateGeneralConsultanCPPTRequest(GeneralConsultanCPPTDto GeneralConsultanCPPTDto) : IRequest<GeneralConsultanCPPTDto>
        {
            public GeneralConsultanCPPTDto GeneralConsultanCPPTDto { get; set; } = GeneralConsultanCPPTDto;
        }

        public class CreateListGeneralConsultanCPPTRequest(List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion Create

        #region Update

        public class UpdateGeneralConsultanCPPTRequest(GeneralConsultanCPPTDto GeneralConsultanCPPTDto) : IRequest<GeneralConsultanCPPTDto>
        {
            public GeneralConsultanCPPTDto GeneralConsultanCPPTDto { get; set; } = GeneralConsultanCPPTDto;
        }

        public class UpdateListGeneralConsultanCPPTRequest(List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public List<GeneralConsultanCPPTDto> GeneralConsultanCPPTDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanCPPTRequest(Guid? id = null, List<Guid>? ids = null, Guid? deleteByGeneralServiceId = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public Guid DeleteByGeneralServiceId { get; set; } = deleteByGeneralServiceId ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}