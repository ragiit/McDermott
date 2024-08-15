using McHealthCare.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class TransferStock : BaseAuditableEntity
    {
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
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
        public List<TransferStockProduct>? TransferStockProduct { get; set; }

        [SetToNull]
        public List<TransferStockLog>? TransferStockLog { get; set; }
    }
}