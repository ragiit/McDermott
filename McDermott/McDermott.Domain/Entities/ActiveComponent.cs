namespace McDermott.Domain.Entities
{
    public class ActiveComponent : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? AmountOfComponent { get; set; }
    }
}
