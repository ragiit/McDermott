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
        public string? KodeTransaksi { get; set; }
        public string? Reference { get; set; }
        public string? StatusReceived { get; set; }

        [SetToNull]
        public Location? Destination { get; set; }

        [SetToNull]
        public List<ReceivingStockProduct>? receivingStockProduct { get; set; }
    }
}