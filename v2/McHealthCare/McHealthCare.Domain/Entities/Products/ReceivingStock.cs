using McHealthCare.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class ReceivingStock : BaseAuditableEntity
    {
        public Guid? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; }

        public string? KodeReceiving { get; set; }
        public string? NumberPurchase { get; set; }
        public string? Reference { get; set; }

        public EnumStatusReceiving? Status { get; set; }

        [SetToNull]
        public Location? Destination { get; set; }

        [SetToNull]
        public List<ReceivingStockProduct>? receivingStockProduct { get; set; }
    }
}