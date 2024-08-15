namespace McHealthCare.Domain.Entities.Inventory
{
    public class Location : BaseAuditableEntity
    {
        public Guid? ParentLocationId { get; set; }
        public Guid? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        public virtual Location? ParentLocation { get; set; }
        public virtual Company? Company { get; set; }
    }
}