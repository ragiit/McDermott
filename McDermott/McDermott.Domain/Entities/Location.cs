namespace McDermott.Domain.Entities
{
    public class Location : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        public virtual List<BuildingLocation>? BuildingLocations { get; set; }
    }
}