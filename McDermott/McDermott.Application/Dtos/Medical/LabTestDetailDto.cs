namespace McDermott.Application.Dtos.Medical
{
    public class LabTestDetailDto : IMapFrom<LabTestDetail>
    {
        public long Id { get; set; }

        public long? LabTestId { get; set; }
        public long? LabUomId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [NotMapped]
        public string? NormalRangeByGender { get; set; } = string.Empty;

        public LabTestDto? LabTest { get; set; }
        public LabUomDto? LabUom { get; set; }
    }
}
