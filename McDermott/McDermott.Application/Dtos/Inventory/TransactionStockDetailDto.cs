using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransactionStockDetailDto : IMapFrom<TransactionStockDetail>
    {
        public long Id { get; set; }
        public long? TransactionStockId { get; set; }       
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? TypeTransaction { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }

        [SetToNull]
        public virtual LocationDto? Source { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }
    }
}