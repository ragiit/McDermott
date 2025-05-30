﻿namespace McHealthCare.Domain.Entities.Products
{
    public class ReceivingStockProduct : BaseAuditableEntity
    {
        public Guid? ReceivingStockId { get; set; }
        public Guid? ProductId { get; set; }
        public long? Qty { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public ReceivingStock? ReceivingStock { get; set; }
        public Product? Product { get; set; }

        public StockProduct? Stock { get; set; }
    }
}