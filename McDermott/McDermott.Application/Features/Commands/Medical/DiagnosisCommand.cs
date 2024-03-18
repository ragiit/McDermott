namespace McDermott.Application.Features.Commands.Medical
{
    public class DiagnosisCommand
    {
        public class GetDiagnosisQuery : IRequest<List<DiagnosisDto>>;

        public class GetDiagnosisByIdQuery : IRequest<DiagnosisDto>
        {
            public long Id { get; set; }

            public GetDiagnosisByIdQuery(long id)
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
            public long Id { get; set; }

            public DeleteDiagnosisRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListDiagnosisRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDiagnosisRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}