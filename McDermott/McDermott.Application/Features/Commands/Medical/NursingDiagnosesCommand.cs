using McDermott.Application.Interfaces.Repositories;
using static McDermott.Application.Features.Commands.Medical.NursingDiagnosesCommand;

namespace McDermott.Application.Features.Commands.Medical
{
    public class NursingDiagnosesCommand
    {
        #region GET

        public class GetNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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