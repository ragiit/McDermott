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
        public long? ReceivingId { get; set; }
        public long? PrescriptionId { get; set; }
        public long? ConcoctionLineId { get; set; }
        public long? TransferId { get; set; }
        public long? AdjustmentsId { get; set; }
        public long? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? UomId { get; set; }
        public bool Validate { get; set; } = false;
        public long InStock { get; set; } = 0;
        public long OutStock { get; set; } = 0;


        public virtual ReceivingStockDto? Receiving { get; set; }
        public virtual PrescriptionDto? Prescription { get; set; }
        public virtual ConcoctionLineDto? ConcoctionLine { get; set; }
        public virtual TransferStockDto? Transfer { get; set; }
        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destination { get; set; }
        public virtual Uom? Uom { get; set; }
        public virtual InventoryAdjusmentDto? InventoryAdjusment { get; set; }
    }
}
