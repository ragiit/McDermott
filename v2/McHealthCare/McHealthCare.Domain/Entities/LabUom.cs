namespace McHealthCare.Domain.Entities
{
    public class LabUom : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}
