using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class TransferStock : BaseAuditableEntity
    {
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? KodeTransaksi { get; set; }
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? Reference { get; set; }
        public bool? StockRequest { get; set; }

        public Location? Source { get; set; }

        public Location? Destination { get; set; }

        public List<TransferStockProduct>? TransferStockProduct { get; set; }

        //public List<TransferStockLog>? TransferStockLog { get; set; }
    }
}