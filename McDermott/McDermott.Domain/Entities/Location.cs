namespace McDermott.Domain.Entities
{
    public class Location : BaseAuditableEntity
    {
        public long? ParentLocationId { get; set; }
        public long? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        [SetToNull]
        public virtual List<BuildingLocation>? BuildingLocations { get; set; }

        [SetToNull]
        public virtual List<ReorderingRule>? ReorderingRules { get; set; }

        [SetToNull]
        public virtual Location? ParentLocation { get; set; }

        [SetToNull]
        public virtual Company? Company { get; set; }

        [SetToNull]
        public List<TransactionStockDetail>? TransactionStockDetail { get; set; }
    }
}