namespace McDermott.Application.Features.Commands.Medical
{
    public class ProcedureCommand
    {
        #region GET

        public class GetProcedureQuery(Expression<Func<Procedure, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<ProcedureDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Procedure, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class BulkValidateProcedureQuery(List<ProcedureDto> ProceduresToValidate) : IRequest<List<ProcedureDto>>
        {
            public List<ProcedureDto> ProceduresToValidate { get; } = ProceduresToValidate;
        }

        public class ValidateProcedureQuery(Expression<Func<Procedure, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Procedure, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateProcedureRequest(ProcedureDto ProcedureDto) : IRequest<ProcedureDto>
        {
            public ProcedureDto ProcedureDto { get; set; } = ProcedureDto;
        }

        public class CreateListProcedureRequest(List<ProcedureDto> ProcedureDtos) : IRequest<List<ProcedureDto>>
        {
            public List<ProcedureDto> ProcedureDtos { get; set; } = ProcedureDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProcedureRequest(ProcedureDto ProcedureDto) : IRequest<ProcedureDto>
        {
            public ProcedureDto ProcedureDto { get; set; } = ProcedureDto;
        }

        public class UpdateListProcedureRequest(List<ProcedureDto> ProcedureDtos) : IRequest<List<ProcedureDto>>
        {
            public List<ProcedureDto> ProcedureDtos { get; set; } = ProcedureDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProcedureRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}