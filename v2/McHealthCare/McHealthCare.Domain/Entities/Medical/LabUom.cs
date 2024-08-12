namespace McHealthCare.Domain.Entities.Medical
{
    public partial class LabUom : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}