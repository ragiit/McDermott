namespace McHealthCare.Application.Dtos.Inventory
{
    public class TransactionStockDto : IMapFrom<TransactionStock>
    {
        public Guid Id { get; set; }
        public string? SourceTable { get; set; }
        public Guid? SourcTableId { get; set; }
        public Guid? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? UomId { get; set; }
        public bool Validate { get; set; } = false;
        public long Quantity { get; set; } = 0;
        public DateTime? CreatedDate { get; set; } = null;

        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Location { get; set; }
        public virtual Uom? Uom { get; set; }
    }
}