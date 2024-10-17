using System;
using System.Collections.Generic;
namespace McDermott.Domain.Entities
{
    public class GoodsReceiptLog :BaseAuditableEntity
    {
        public long? GoodsReceiptId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }

        public GoodsReceipt? GoodsReceipt {  get; set; }
        public User? UserBy { get; set; }
        public Locations? Source { get; set; }
        public Locations? Destination { get; set; }
    }
}
