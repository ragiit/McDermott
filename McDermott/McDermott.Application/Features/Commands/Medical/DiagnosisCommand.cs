namespace McDermott.Application.Features.Commands.Medical
{
    public class DiagnosisCommand
    {
        #region GET

        public class GetDiagnosisQuery(Expression<Func<Diagnosis, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<DiagnosisDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Diagnosis, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class BulkValidateDiagnosisQuery(List<DiagnosisDto> DiagnosissToValidate) : IRequest<List<DiagnosisDto>>
        {
            public List<DiagnosisDto> DiagnosissToValidate { get; } = DiagnosissToValidate;
        } 

        public class ValidateDiagnosisQuery(Expression<Func<Diagnosis, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Diagnosis, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET
         
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