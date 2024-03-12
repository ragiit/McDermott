namespace McDermott.Domain.Entities
{
    public partial class Building : BaseAuditableEntity
    {
        public long? HealthCenterId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Code { get; set; } = string.Empty;

        [SetToNull]
        public virtual HealthCenter? HealthCenter { get; set; }

        [SetToNull]
        public virtual List<BuildingLocation>? BuildingLocations { get; set; }
    }
}