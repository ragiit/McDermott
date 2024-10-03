using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    public class Locations : BaseAuditableEntity
    {
        public long? ParentLocationId { get; set; }
        public long? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        [NotMapped]
        public string ParentLocationString => ParentLocation is not null ? $"{ParentLocation.Name}/{Name}" : string.Empty;

        [SetToNull]
        public virtual List<BuildingLocation>? BuildingLocations { get; set; }

        [SetToNull]
        public virtual List<ReorderingRule>? ReorderingRules { get; set; }

        [SetToNull]
        public virtual Locations? ParentLocation { get; set; }

        [SetToNull]
        public virtual Company? Company { get; set; }

        [SetToNull]
        public List<TransferStockLog>? TransferStockLog { get; set; }
    }
}