namespace McHealthCare.Domain.Entities.Medical{
    public partial class LabTestDetail : BaseAuditableEntity{
        public Guid? LabTestId { get; set; }
        public Guid? LabUomId { get; set; }
        public string? Name { get; set; }
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