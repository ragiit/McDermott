namespace McDermott.Domain.Entities
{
    public class InventoryAdjusment : BaseAuditableEntity
    {
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public EnumStatusInventoryAdjustment Status { get; set; }

        [SetToNull]
        public virtual Locations? Location { get; set; }

        [SetToNull]
        public virtual Company? Company { get; set; }

        public virtual IEnumerable<InventoryAdjusmentDetail>? InventoryAdjusmentDetails { get; set; }
    }
}