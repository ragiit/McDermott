using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransactionStockDto:IMapFrom<TransactionStock>
    {
        private long _initialStock = 0;
        public long InitialStock
        {
            get { return _initialStock; }
            set
            {
                _initialStock = value;
                CalculateEndStock();
            }
        }

        private long _inStock = 0;
        public long InStock
        {
            get { return _inStock; }
            set
            {
                _inStock = value;
                CalculateEndStock();
            }
        }

        private long _outStock = 0;
        public long OutStock
        {
            get { return _outStock; }
            set
            {
                _outStock = value;
                CalculateEndStock();
            }
        }

        private long _endStock;
        public long EndStock
        {
            get { return _endStock; }
            private set { _endStock = value; }
        }

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

        private void CalculateEndStock()
        {
            EndStock = InitialStock + InStock - OutStock;
        }

        public virtual ReceivingStockDto? Receiving { get; set; }
        public virtual PrescriptionDto? Prescription { get; set; }
        public virtual ConcoctionLineDto? ConcoctionLine { get; set; }
        public virtual TransferStockDto? Transfer { get; set; }
        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destination { get; set; }
        public virtual Uom? Uom { get; set; }
        public virtual InventoryAdjusmentDto? InventoryAdjusment { get;set; }
    }
}
