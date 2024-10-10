namespace McDermott.Domain.Entities
{
    public class GeneralConsultanCPPT : BaseAuditableEntity
    {
        public long GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public long? UserId { get; set; }
        public string? Subjective { get; set; }
        public string? Objective { get; set; }
        public long? DiagnosisId { get; set; }
        public long? NursingDiagnosesId { get; set; }
        public string? Planning { get; set; }

        public User? User { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        public NursingDiagnoses? NursingDiagnoses { get; set; }
        public virtual GeneralConsultanService? GeneralConsultanService { get; set; }
    }
}