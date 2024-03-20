namespace McDermott.Domain.Entities
{
    public class LabTest : BaseAuditableEntity
    {
        public long? SampleTypeId { get; set; }
        public long? LabUomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [SetToNull]
        public SampleType? SampleType { get; set; }
        [SetToNull]
        public LabUom? LabUom { get; set; }
    }
}
