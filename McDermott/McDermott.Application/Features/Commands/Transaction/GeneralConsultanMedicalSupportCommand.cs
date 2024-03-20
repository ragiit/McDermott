namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanMedicalSupportCommand
    {
        #region Get

        public class GetGeneralConsultanMedicalSupportQuery(Expression<Func<GeneralConsultanMedicalSupport, bool>>? predicate = null, bool RemoveCache = false) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = RemoveCache;
        }

        #endregion Get

        #region Create

        public class CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
        }

        public class CreateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
        }

        #endregion Create

        #region Update

        public class UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
        {
            public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
        }

        public class UpdateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
        }
        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanMedicalSupportRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}