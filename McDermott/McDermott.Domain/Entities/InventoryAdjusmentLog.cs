namespace McDermott.Domain.Entities
{
    public class InventoryAdjusmentLog : BaseAuditableEntity
    {
        public long? InventoryAdjusmentId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusInventoryAdjustment Status { get; set; }

        public InventoryAdjusment? InventoryAdjusment { get; set; }
        public User? UserBy { get; set; }
    }
}