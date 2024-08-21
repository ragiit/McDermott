using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransferStockDto : IMapFrom<TransferStock>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Select Source Location... ")]
        public long? SourceId { get; set; }

        [Required(ErrorMessage = "Please Select Destination Location... ")]
        public long? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? KodeTransaksi { get; set; }
        public bool? StockRequest { get; set; } = false;
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? Reference { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }

        [SetToNull]
        public virtual LocationDto? Source { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }
    }
}