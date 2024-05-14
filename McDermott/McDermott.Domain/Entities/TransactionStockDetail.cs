using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStockDetail : BaseAuditableEntity
    {
        public long? TransactionStockId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public string? StatusTransfer { get; set; }
        public string? TypeTransaction { get; set; }

        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }

        [SetToNull]
        public Location? Source { get; set; }

        [SetToNull]
        public Location? Destination { get; set; }
    }
}