namespace McDermott.Domain.Entities
{
    public class LabTestDetail : BaseAuditableEntity
    {
        public long? LabTestId { get; set; }
        public long? LabUomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [SetToNull]
        public LabTest? LabTest { get; set; }

        [SetToNull]
        public LabUom? LabUom { get; set; }
    }
}