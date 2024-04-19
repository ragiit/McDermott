namespace McDermott.Domain.Entities
{
    public class LabResultDetail : BaseAuditableEntity
    {
        public long GeneralConsultanMedicalSupportId { get; set; }
        public long? LabTestId { get; set; }

        public string? Result { get; set; }
        public string? ResultType { get; set; }

        [SetToNull]
        public LabTestDetail? LabTest { get; set; } = new();

        [SetToNull]
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
    }
}
