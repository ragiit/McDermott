namespace McDermott.Domain.Entities
{
    public class GoodsReceipt : BaseAuditableEntity
    {
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? NumberPurchase { get; set; }
        public string? Reference { get; set; }

        public EnumStatusGoodsReceipt? Status { get; set; }

        [SetToNull]
        public Locations? Destination { get; set; }

        [SetToNull]
        public Locations? Source { get; set; }

        [SetToNull]
        public List<GoodsReceiptDetail>? goodsReceiptDetails { get; set; }
    }
}