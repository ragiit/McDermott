namespace McDermott.Application.Features.Commands.Transaction
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

        public class DeleteGeneralConsultanCPPTRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}