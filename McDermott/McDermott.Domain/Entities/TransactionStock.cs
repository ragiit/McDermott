using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStock : BaseAuditableEntity
    {
        public long? StockId { get; set; }
        public long? ProductId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? QtyStock { get; set; }
        public DateTime? SchenduleDate { get; set; }
        public string? KodeTransaksi { get; set; }
        public string? StatusStock { get; set; }
        public string? StatusTranfer { get; set; }
        public string? Reference { get; set; }

        public StockProduct? Stock { get; set; }
        public Product? Product { get; set; }
        public Location? Source { get; set; }
        public Location? Destination { get; set; }
    }
}