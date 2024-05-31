namespace McDermott.Domain.Entities
{
    public class InventoryAdjusment : BaseAuditableEntity
    {
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public EnumStatusInventoryAdjusment Status { get; set; }

        public virtual Location? Location { get; set; }
        public virtual Company? Company { get; set; }

        public virtual IEnumerable<InventoryAdjusmentDetail>? InventoryAdjusmentDetails { get; set; }
    }
}
