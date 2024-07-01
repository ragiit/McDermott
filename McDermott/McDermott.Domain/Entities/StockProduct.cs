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
        public long? SourceId { get; set; }
        public long? DestinanceId { get; set; }
        public long? UomId { get; set; }
        public string? StatusTransaction { get; set; }
        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? Expired { get; set; }

        [SetToNull]
        public Product? Product { get; set; }
        [SetToNull]
        public Location? Source { get; set; }
        [SetToNull]
        public Location? Destinance { get; set; }
        [SetToNull]
        public Uom? Uom { get; set; }


        [SetToNull]
        public List<TransactionStock>? TransactionStocks { get; set; }
        [SetToNull]
        public List<ReceivingStockProduct>? ReceivingStockProduct { get; set; }
    }
}