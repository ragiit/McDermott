namespace McDermott.Application.Features.Commands.Medical
{
    public class ProcedureCommand
    {
        public class GetProcedureQuery : IRequest<List<ProcedureDto>>;

        public class GetProcedureByIdQuery : IRequest<ProcedureDto>
        {
             public long Id { get; set; }

            public GetProcedureByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateProcedureRequest : IRequest<ProcedureDto>
        {
            public ProcedureDto ProcedureDto { get; set; }

            public CreateProcedureRequest(ProcedureDto ProcedureDto)
            {
                this.ProcedureDto = ProcedureDto;
            }
        }

        public class UpdateProcedureRequest : IRequest<bool>
        {
            public ProcedureDto ProcedureDto { get; set; }

            public UpdateProcedureRequest(ProcedureDto ProcedureDto)
            {
                this.ProcedureDto = ProcedureDto;
            }
        }

        public class DeleteProcedureRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteProcedureRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListProcedureRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListProcedureRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}