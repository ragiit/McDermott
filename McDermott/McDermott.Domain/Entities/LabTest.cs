namespace McDermott.Domain.Entities
{
    public class LabTest : BaseAuditableEntity
    {
        public long? SampleTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? ResultType { get; set; }

        [SetToNull]
        public virtual SampleType? SampleType { get; set; }

        [SetToNull]
        public virtual List<LabTestDetail>? LabTestDetails { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }
    }
}