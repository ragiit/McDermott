﻿using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class GoodsReceiptDetailDto : IMapFrom<GoodsReceiptDetail>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public long? ProductId { get; set; }

        public long? GoodsReceiptId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long Qty { get; set; } = 0;

        public bool TraceAbility { get; set; } = false;
        public DateTime? ExpiredDate { get; set; }

        public string? UomName { get; set; }
        public string? ProductName { get; set; }
        public string? PurchaseName { get; set; }
        public string? Batch { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }

        [SetToNull]
        public virtual StockProductDto? Stock { get; set; }

        [SetToNull]
        public virtual GoodsReceiptDto? GoodsReceipt { get; set; }
    }
}