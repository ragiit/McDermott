using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ReceivingStockProductDto : IMapFrom<ReceivingStockProduct>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public long? ProductId { get; set; }

        public long? ReceivingStockId { get; set; }

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
        public virtual ReceivingStockDto? ReceivingStock { get; set; }
    }
}