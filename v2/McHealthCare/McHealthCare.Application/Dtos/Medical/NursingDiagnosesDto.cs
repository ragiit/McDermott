using Mapster;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class NursingDiagnosesDto : IMapFrom<NursingDiagnoses>
    {
        public Guid Id { get; set; }
        public string? Problem { get; set; }
        public string? Code { get; set; }
    }

    public class CreateUpdateNursingDiagnosesDto : IMapFrom<NursingDiagnoses>
    {
        public Guid Id { get; set; }
        public string? Problem { get; set; }
        public string? Code { get; set; }
    }
}