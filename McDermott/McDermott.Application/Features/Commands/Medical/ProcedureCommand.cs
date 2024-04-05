namespace McDermott.Application.Features.Commands.Medical
{
    public class ProcedureCommand
    {
        #region GET 

        public class GetProcedureQuery(Expression<Func<Procedure, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProcedureDto>>
        {
            public Expression<Func<Procedure, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

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