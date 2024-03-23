namespace McDermott.Domain.Entities
{
    public class LabResultDetail : BaseAuditableEntity
    {
        public long GeneralConsultanMedicalSupportId { get; set; }
        public long? LabTestId { get; set; }
        public string? ResultValueType { get; set; }

        [SetToNull]
        public LabTest? LabTest { get; set; } = new();

        [SetToNull]
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
    }
}
