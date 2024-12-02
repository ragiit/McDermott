namespace McDermott.Application.Features.Commands.Medical
{
    public class DiagnosisCommand
    {
        #region GET

        public class GetSingleDiagnosisQuery : IRequest<DiagnosisDto>
        {
            public List<Expression<Func<Diagnosis, object>>> Includes { get; set; }
            public Expression<Func<Diagnosis, bool>> Predicate { get; set; }
            public Expression<Func<Diagnosis, Diagnosis>> Select { get; set; }

            public List<(Expression<Func<Diagnosis, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDiagnosisQuery : IRequest<(List<DiagnosisDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Diagnosis, object>>> Includes { get; set; }
            public Expression<Func<Diagnosis, bool>> Predicate { get; set; }
            public Expression<Func<Diagnosis, Diagnosis>>? Select { get; set; }

            public List<(Expression<Func<Diagnosis, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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