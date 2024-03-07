namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanCPPTCommand
    {
        #region Get

        public class GetGeneralConsultanCPPTQuery(Expression<Func<GeneralConsultanCPPT, bool>>? predicate = null) : IRequest<List<GeneralConsultanCPPTDto>>
        {
            public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; } = predicate;
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

        public class UpdateGeneralConsultanCPPTRequest(GeneralConsultanCPPTDto GeneralConsultanCPPTDto) : IRequest<bool>
        {
            public GeneralConsultanCPPTDto GeneralConsultanCPPTDto { get; set; } = GeneralConsultanCPPTDto;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanCPPTRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}