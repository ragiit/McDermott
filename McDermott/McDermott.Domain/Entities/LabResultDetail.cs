namespace McDermott.Domain.Entities
{
    public class LabResultDetail : BaseAuditableEntity
    {
        public long GeneralConsultanMedicalSupportId { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRange { get; set; } = string.Empty;
        public long? LabUomId { get; set; }
        public string? Result { get; set; }
        public string? ResultType { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [SetToNull]
        public LabUom? LabUom { get; set; }

        [SetToNull]
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
    }
}