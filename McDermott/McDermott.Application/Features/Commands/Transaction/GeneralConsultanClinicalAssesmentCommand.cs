namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanClinicalAssesmentCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetGeneralConsultantClinicalAssesmentQuery(Expression<Func<GeneralConsultantClinicalAssesment, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GeneralConsultantClinicalAssesmentDto>>
        {
            public Expression<Func<GeneralConsultantClinicalAssesment, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        #endregion

        #region CREATE
        public class CreateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto) : IRequest<GeneralConsultantClinicalAssesmentDto>
        {
            public GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto { get; set; } = GeneralConsultantClinicalAssesmentDto;
        }

        public class CreateListGeneralConsultantClinicalAssesmentRequest(List<GeneralConsultantClinicalAssesmentDto> GeneralConsultanCPPTDtos) : IRequest<List<GeneralConsultantClinicalAssesmentDto>>
        {
            public List<GeneralConsultantClinicalAssesmentDto> GeneralConsultantClinicalAssesmentDtos { get; set; } = GeneralConsultanCPPTDtos;
        }
        #endregion

        #region Update
        public class UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto) : IRequest<GeneralConsultantClinicalAssesmentDto>
        {
            public GeneralConsultantClinicalAssesmentDto GeneralConsultantClinicalAssesmentDto { get; set; } = GeneralConsultantClinicalAssesmentDto;
        }

        public class UpdateListGeneralConsultantClinicalAssesmentRequest(List<GeneralConsultantClinicalAssesmentDto> GeneralConsultantClinicalAssesmentDtos) : IRequest<List<GeneralConsultantClinicalAssesmentDto>>
        {
            public List<GeneralConsultantClinicalAssesmentDto> GeneralConsultantClinicalAssesmentDtos { get; set; } = GeneralConsultantClinicalAssesmentDtos;
        }

        #endregion

        #region DELETE 
        public class DeleteGeneralConsultantClinicalAssesmentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
