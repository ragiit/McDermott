using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class ReceivingStockDetailDto : IMapFrom<ReceivingStockDetail>
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? StockId { get; set; }
        public long? DestinationId { get; set; }
        public long? Qty { get; set; }
        private bool TraceAbility { get; set; } = false;
        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public DateTime? ExpiredDate { get; set; } = DateTime.Now;
        public string? References { get; set; }
        public string? UomName { get; set; }
        public string? ProductName { get; set; }
        public string? PurchaseName { get; set; }
        public string? Batch { get; set; }

        public virtual ProductDto? Product { get; set; }
        public virtual StockProductDto? Stock { get; set; }
    }
}