namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanMedicalSupportCommand
    {
        #region Get

        public class GetGeneralConsultanMedicalSupportQuery(Expression<Func<GeneralConsultanMedicalSupport, bool>>? predicate = null) : IRequest<List<GeneralConsultanMedicalSupportDto>>
        {
            public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; } = predicate;
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

        public class UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<bool>
        {
            public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanMedicalSupportRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}