using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class StockProduct : BaseAuditableEntity
    {
        public long? ProductId { get; set; }
        public long? Qty { get; set; }
        public DateTime? Expired { get; set; }
        public long? SourceId { get; set; }
        public long? DestinanceId { get; set; }
        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public bool? StatusTransaction { get; set; }

        public Product? Product { get; set; }
        public Location? Source { get; set; }
        public Location? Destinance { get; set; }
    }
}
