namespace McHealthCare.Application.Dtos.Inventory
{
    public class TransferStockDto : IMapFrom<TransferStock>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please Select Source Location... ")]
        public Guid? SourceId { get; set; }

        [Required(ErrorMessage = "Please Select Destination Location... ")]
        public Guid? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? KodeTransaksi { get; set; }
        public bool? StockRequest { get; set; } = false;
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? Reference { get; set; }

        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destination { get; set; }
    }
}