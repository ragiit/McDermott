namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Building : BaseAuditableEntity
    {
        public Guid? HealthCenterId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        [SetToNull]
        public virtual HealthCenter? HealthCenter { get; set; }
        [SetToNull]
        public virtual List<BuildingLocation>? BuildingLocations { get; set; }
    }
}