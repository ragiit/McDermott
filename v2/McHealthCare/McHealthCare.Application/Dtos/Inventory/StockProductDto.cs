namespace McHealthCare.Application.Dtos.Inventory
{
    public class StockProductDto : IMapFrom<StockProduct>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please Select Product..")]
        public Guid? ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Please input Quantity..")]
        public long Qty { get; set; } = 0;

        public Guid? UomId { get; set; }
        public string? UomName { get; set; }
        public DateTime? Expired { get; set; }

        [Required(ErrorMessage = "Please Select Source Location..")]
        public Guid? SourceId { get; set; }

        public string? SourceName { get; set; }

        [Required(ErrorMessage = "Please Select Destination Location..")]
        public Guid? DestinanceId { get; set; }

        public string? DestinanceName { get; set; }

        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public string? StatusTransaction { get; set; }

        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destinance { get; set; }
        public virtual UomDto? Uom { get; set; }
    }
}