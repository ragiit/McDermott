namespace McDermott.Application.Features.Commands.Medical
{
    public class NursingDiagnosesCommand
    {
        #region GET

        public class GetSingleNursingDiagnosesQuery : IRequest<NursingDiagnosesDto>
        {
            public List<Expression<Func<NursingDiagnoses, object>>> Includes { get; set; }
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; set; }
            public Expression<Func<NursingDiagnoses, NursingDiagnoses>> Select { get; set; }

            public List<(Expression<Func<NursingDiagnoses, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetNursingDiagnosesQuery : IRequest<(List<NursingDiagnosesDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<NursingDiagnoses, object>>> Includes { get; set; }
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; set; }
            public Expression<Func<NursingDiagnoses, NursingDiagnoses>>? Select { get; set; }

            public List<(Expression<Func<NursingDiagnoses, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateNursingDiagnosesQuery(List<NursingDiagnosesDto> NursingDiagnosessToValidate) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosessToValidate { get; } = NursingDiagnosessToValidate;
        }

        public class ValidateNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        public class CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        public class UpdateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteNursingDiagnosesRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}