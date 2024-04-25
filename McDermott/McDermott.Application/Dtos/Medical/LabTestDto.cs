
namespace McDermott.Application.Dtos.Medical
{
    public class LabTestDto : IMapFrom<LabTest>
    {
        public long Id { get; set; }
        public long? SampleTypeId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string ResultType { get; set; } = "Quantitative";

        public virtual SampleTypeDto? SampleType { get; set; }

        public virtual List<GeneralConsultanMedicalSupportDto>? GeneralConsultanMedicalSupports { get; set; }
    }
}
