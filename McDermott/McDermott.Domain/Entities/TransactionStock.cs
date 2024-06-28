using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStock : BaseAuditableEntity
    {
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; }
        public string? KodeTransaksi { get; set; }
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? Reference { get; set; }
        public bool? StockRequest { get; set; }

        [SetToNull]
        public Location? Source { get; set; }

        [SetToNull]
        public Location? Destination { get; set; }

        [SetToNull]
        public List<TransactionStockProduct>? TransactionStockProduct { get; set; }

        [SetToNull]
        public List<TransactionStockDetail>? TransactionStockDetail { get; set; }
    }
}