namespace McHealthCare.Domain.Entities
{
    public class InventoryAdjusmentDetail : BaseAuditableEntity
    {
        public Guid? StockProductId { get; set; }
        public Guid? TransactionStockId { get; set; }
        public Guid InventoryAdjusmentId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public long TeoriticalQty { get; set; } = 0;
        public string? Batch { get; set; }
        public long RealQty { get; set; } = 0;

        public virtual InventoryAdjusment? InventoryAdjusment { get; set; }

        public virtual Product? Product { get; set; }

        public virtual StockProduct? StockProduct { get; set; }

        public virtual TransactionStock? TransactionStock { get; set; }
    }
}