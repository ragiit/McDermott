namespace McHealthCare.Domain.Entities.Medical
{
    public partial class SampleType : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual List<LabTest>? LabTests { get; set; }
    }
}