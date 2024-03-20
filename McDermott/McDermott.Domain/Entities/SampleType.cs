namespace McDermott.Domain.Entities
{
    public class SampleType : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
