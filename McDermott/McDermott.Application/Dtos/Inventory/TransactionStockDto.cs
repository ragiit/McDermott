using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransactionStockDto : IMapFrom<TransactionStock>
    {
        
        public long Id { get; set; }
        public string? SourceTable { get; set; }
        public long? SourcTableId { get; set; }
        public long? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? UomId { get; set; }
        public bool Validate { get; set; } = false;
        public long Quantity { get; set; } = 0;


        
        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destination { get; set; }
        public virtual Uom? Uom { get; set; }
        public virtual InventoryAdjusmentDto? InventoryAdjusment { get; set; }
    }
}
