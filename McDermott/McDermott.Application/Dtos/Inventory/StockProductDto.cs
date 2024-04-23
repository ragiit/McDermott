using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class StockProductDto :IMapFrom<StockProduct>
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? Qty { get; set; }
        public DateTime? Expired { get; set; }
        public long? SourceId { get; set; }
        public long? DestinanceId { get; set; }
        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public bool? StatusTransaction { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }
        [SetToNull]
        public virtual LocationDto? Source { get; set; }
        [SetToNull]
        public virtual LocationDto? Destinance { get; set; }
    }
}
