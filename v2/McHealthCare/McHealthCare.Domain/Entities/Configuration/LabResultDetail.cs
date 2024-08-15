namespace McHealthCare.Domain.Entities
{
    public class LabResultDetail : BaseAuditableEntity
    {
        public Guid GeneralConsultanMedicalSupportId { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRange { get; set; } = string.Empty;
        public Guid? LabUomId { get; set; }
        public string? Result { get; set; }
        public string? ResultType { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        //
        //public LabTestDetail? LabTestDetail { get; set; } = new();

        public LabUom? LabUom { get; set; }
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
    }
}