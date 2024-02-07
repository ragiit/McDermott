using McDermott.Application.Dtos.Medical;

namespace McDermott.Application.Features.Commands.Medical
{
    public class ProcedureCommand
    {
        public class GetProcedureQuery : IRequest<List<ProcedureDto>>;

        public class GetProcedureByIdQuery : IRequest<ProcedureDto>
        {
            public int Id { get; set; }

            public GetProcedureByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteProcedureRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListProcedureRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListProcedureRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
