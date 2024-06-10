namespace McDermott.Domain.Entities
{
    public class InventoryAdjusmentDetail : BaseAuditableEntity
    {
        public long? StockProductId { get; set; }
        public long InventoryAdjusmentId { get; set; }
        public long? ProductId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public long RealQty { get; set; } = 0;

        [SetToNull]
        public virtual InventoryAdjusment? InventoryAdjusment { get; set; }
        [SetToNull]
        public virtual Product? Product { get; set; }
        [SetToNull]
        public virtual StockProduct? StockProduct { get; set; }
    }
}
