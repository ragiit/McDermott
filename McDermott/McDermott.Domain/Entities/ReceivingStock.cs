using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ReceivingStock : BaseAuditableEntity
    {
        public long? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; }

        public string? KodeReceiving { get; set; }
        public string? NumberPurchase { get; set; }
        public string? Reference { get; set; }

        public EnumStatusReceiving? Status { get; set; }

        [SetToNull]
        public Locations? Destination { get; set; }

        [SetToNull]
        public List<ReceivingStockProduct>? receivingStockProduct { get; set; }
    }
}