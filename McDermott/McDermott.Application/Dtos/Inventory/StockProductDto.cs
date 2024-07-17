using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class StockProductDto : IMapFrom<StockProduct>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Select Product..")]
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Please input Quantity..")]
        public long Qty { get; set; } = 0;

        public long? UomId { get; set; }
        public string? UomName { get; set; }
        public DateTime? Expired { get; set; }

        [Required(ErrorMessage = "Please Select Source Location..")]
        public long? SourceId { get; set; }
        public string? SourceName { get; set; }

        [Required(ErrorMessage = "Please Select Destination Location..")]
        public long? DestinanceId { get; set; }
        public string? DestinanceName { get; set; }

        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public string? StatusTransaction { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }

        [SetToNull]
        public virtual LocationDto? Source { get; set; }

        [SetToNull]
        public virtual LocationDto? Destinance { get; set; }

        [SetToNull]
        public virtual UomDto? Uom { get; set; }
    }
}