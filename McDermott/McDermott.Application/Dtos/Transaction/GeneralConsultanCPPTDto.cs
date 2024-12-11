namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanCPPTDto : IMapFrom<GeneralConsultanCPPT>
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public DateTime CreatedDate { get; set; }

        public long? UserId { get; set; }
        public string? Subjective { get; set; }
        public string? Objective { get; set; }
        public long? DiagnosisId { get; set; }
        public long? NursingDiagnosesId { get; set; }
        public string? Planning { get; set; }
        public string? Anamnesa { get; set; }
        public string? MedicationTherapy { get; set; }
        public string? NonMedicationTherapy { get; set; }
        public UserDto? User { get; set; }
        public DiagnosisDto? Diagnosis { get; set; }
        public NursingDiagnosesDto? NursingDiagnoses { get; set; }
        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }

    public class CreateUpdateGeneralConsultanCPPTDto
    {
        public long Id { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public DateTime CreatedDate { get; set; }

        public long? UserId { get; set; }
        public string? Subjective { get; set; }
        public string? Objective { get; set; }
        public long? DiagnosisId { get; set; }
        public long? NursingDiagnosesId { get; set; }
        public string? Planning { get; set; }
        public string? Anamnesa { get; set; }
        public string? MedicationTherapy { get; set; }
        public string? NonMedicationTherapy { get; set; }
    }
}