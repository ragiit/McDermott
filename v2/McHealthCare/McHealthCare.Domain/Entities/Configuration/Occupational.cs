namespace McHealthCare.Domain.Entities.Configuration
{
    public class Occupational : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
    }
}