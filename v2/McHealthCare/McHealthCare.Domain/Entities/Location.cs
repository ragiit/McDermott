namespace McHealthCare.Domain.Entities
{
    public class Location : BaseAuditableEntity
    {
        public Guid? ParentLocationId { get; set; }
        public Guid? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        public virtual List<BuildingLocation>? BuildingLocations { get; set; }

        public virtual List<ReorderingRule>? ReorderingRules { get; set; }

        public virtual Location? ParentLocation { get; set; }

        public virtual Company? Company { get; set; }

        //public virtual List<TransferStockLog>? TransferStockLogs { get; set; }
    }
}