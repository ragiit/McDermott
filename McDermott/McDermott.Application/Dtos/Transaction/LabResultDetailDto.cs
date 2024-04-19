namespace McDermott.Application.Dtos.Transaction
{
    public class LabResultDetailDto : IMapFrom<LabResultDetail>
    {
        public long Id { get; set; }
        public long GeneralConsultanMedicalSupportId { get; set; }
        public long? LabTestId { get; set; }
        public string? Result { get; set; }
        public string? ResultType { get; set; }

        public LabTestDetailDto? LabTest { get; set; }

        public GeneralConsultanMedicalSupportDto? GeneralConsultanMedicalSupport { get; set; }
    }
}
