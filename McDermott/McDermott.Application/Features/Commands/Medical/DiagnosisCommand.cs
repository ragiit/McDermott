namespace McDermott.Application.Features.Commands.Medical
{
    public class DiagnosisCommand
    {
        public class GetDiagnosisQuery : IRequest<List<DiagnosisDto>>;

        public class GetDiagnosisByIdQuery : IRequest<DiagnosisDto>
        {
            public int Id { get; set; }

            public GetDiagnosisByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateDiagnosisRequest : IRequest<DiagnosisDto>
        {
            public DiagnosisDto DiagnosisDto { get; set; }

            public CreateDiagnosisRequest(DiagnosisDto DiagnosisDto)
            {
                this.DiagnosisDto = DiagnosisDto;
            }
        }

        public class UpdateDiagnosisRequest : IRequest<bool>
        {
            public DiagnosisDto DiagnosisDto { get; set; }

            public UpdateDiagnosisRequest(DiagnosisDto DiagnosisDto)
            {
                this.DiagnosisDto = DiagnosisDto;
            }
        }

        public class DeleteDiagnosisRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDiagnosisRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListDiagnosisRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDiagnosisRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}