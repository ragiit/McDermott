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
        public long? StockId { get; set; }

        [Required(ErrorMessage = "Please Select Source Location... ")]
        public long? SourceId { get; set; }

        [Required(ErrorMessage = "Please Select Source Location... ")]
        public long? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? KodeTransaksi { get; set; }
        public string? StatusTranfer { get; set; }
        public string? Reference { get; set; }

        public virtual StockProductDto? Stock { get; set; }
        public virtual ProductDto? Product { get; set; }
        public virtual LocationDto? Source { get; set; }
        public virtual LocationDto? Destination { get; set; }
    }
}