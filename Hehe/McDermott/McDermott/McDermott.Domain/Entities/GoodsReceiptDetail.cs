using System;
using System.Collections.Generic;
namespace McDermott.Domain.Entities
{
    public class GoodsReceiptDetail : BaseAuditableEntity
    {
        public long? GoodsReceiptId { get; set; }
        public long? ProductId { get; set; }
        public long? Qty { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [SetToNull]
        public GoodsReceipt? GoodsReceipt { get; set; }

        [SetToNull]
        public Product? Product { get; set; }

        [SetToNull]
        public StockProduct? Stock { get; set; }
    }
}