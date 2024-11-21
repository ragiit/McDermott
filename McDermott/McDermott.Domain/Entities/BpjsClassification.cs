namespace McDermott.Domain.Entities
{
    public class BpjsClassification : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
    }
}