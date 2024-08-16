namespace McHealthCare.Domain.Entities.Medical
{
    public partial class LabTest : BaseAuditableEntity
    {
        public Guid? SampleTypeId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ResultType { get; set; }

        public virtual SampleType? SampleType { get; set; }

        public virtual List<LabTestDetail>? LabTestDetails { get; set; }

        //
        // public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }
    }
}