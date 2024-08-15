using McHealthCare.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class TransferStockLog : BaseAuditableEntity
    {
        public Guid? TransferStockId { get; set; }        
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public TransferStock? TransferStock { get; set; }

        [SetToNull]
        public Location? Source { get; set; }

        [SetToNull]
        public Location? Destination { get; set; }
    }
}