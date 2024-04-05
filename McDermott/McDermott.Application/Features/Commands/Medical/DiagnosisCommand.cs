namespace McDermott.Application.Features.Commands.Medical
{
    public class DiagnosisCommand
    {
        #region GET 

        public class GetDiagnosisQuery(Expression<Func<Diagnosis, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DiagnosisDto>>
        {
            public Expression<Func<Diagnosis, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateDiagnosisRequest(DiagnosisDto DiagnosisDto) : IRequest<DiagnosisDto>
        {
            public DiagnosisDto DiagnosisDto { get; set; } = DiagnosisDto;
        }

        public class CreateListDiagnosisRequest(List<DiagnosisDto> DiagnosisDtos) : IRequest<List<DiagnosisDto>>
        {
            public List<DiagnosisDto> DiagnosisDtos { get; set; } = DiagnosisDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDiagnosisRequest(DiagnosisDto DiagnosisDto) : IRequest<DiagnosisDto>
        {
            public DiagnosisDto DiagnosisDto { get; set; } = DiagnosisDto;
        }

        public class UpdateListDiagnosisRequest(List<DiagnosisDto> DiagnosisDtos) : IRequest<List<DiagnosisDto>>
        {
            public List<DiagnosisDto> DiagnosisDtos { get; set; } = DiagnosisDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDiagnosisRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}