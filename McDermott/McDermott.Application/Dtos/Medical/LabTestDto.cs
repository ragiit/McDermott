namespace McDermott.Application.Dtos.Medical
{
    public class LabTestDto : IMapFrom<LabTest>
    {
        public long Id { get; set; }
        public long? SampleTypeId { get; set; }
        public long? LabUomId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        public SampleTypeDto? SampleType { get; set; }
        public LabUomDto? LabUom { get; set; }
    }
}
