namespace McDermott.Application.Dtos.Medical
{
    public class NursingDiagnosesDto : IMapFrom<NursingDiagnoses>
    {
        public int Id { get; set; }
        public string Problem { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}