namespace McDermott.Application.Dtos.Transaction
{
    public class LabResultDetailDto : IMapFrom<LabResultDetail>
    {
        public long Id { get; set; }
        public long GeneralConsultanMedicalSupportId { get; set; }
        public long? LabTestId { get; set; }
        public string? ResultValueType { get; set; }

        public LabTestDto? LabTest { get; set; }

        public GeneralConsultanMedicalSupportDto? GeneralConsultanMedicalSupport { get; set; }
    }
}
