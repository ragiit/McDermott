namespace McHealthCare.Domain.Entities.Configuration
{
    public class Religion : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}