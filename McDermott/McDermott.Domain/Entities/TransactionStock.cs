using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStock : BaseAuditableEntity
    {
        public string? SourceTable { get; set; }
        public long? SourcTableId { get; set; }        
        public long? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? Quantity {  get; set; }
        public long? UomId { get; set; }
        public bool? Validate { get; set; }


        public Product? Product { get; set; }
        public Location? Source {  get; set; }
        public Location? Destination { get; set; }
        public Uom? Uom { get; set; }
        public InventoryAdjusment? InventoryAdjusment { get; set; }

    }
}
