namespace McHealthCare.Domain.Entities.Medical
{
    public partial class LabTest : BaseAuditableEntity
    {
        public Guid? SampleTypeId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        public string? ResultType { get; set; }

        [SetToNull]
        public virtual SampleType? SampleType { get; set; }

        [SetToNull]
        public virtual List<LabTestDetail>? LabTestDetails { get; set; }

        // [SetToNull]
        // public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }
    }
}