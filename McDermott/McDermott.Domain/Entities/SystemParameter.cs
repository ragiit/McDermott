namespace McDermott.Domain.Entities
{
    public class SystemParameter : BaseAuditableEntity
    {
        public string Key { get; set; } = string.Empty;
        public string? Value { get; set; }
    }
}
