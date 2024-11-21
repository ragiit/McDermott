namespace McDermott.Domain.Entities
{
    public class Project : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }
    }
}