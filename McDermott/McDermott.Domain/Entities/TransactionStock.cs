using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStock : BaseAuditableEntity
    {
        public long? ReceivingId { get; set; }
        public long? PrescriptionId { get; set; }
        public long? ConcoctionLineId { get; set; }
        public long? TransferId { get; set; }
        public long? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? InitialStock { get; set; }
        public long? InStock {  get; set; }
        public long? OutStock { get; set; }
        public long? EndStock { get; set; }


        public ReceivingStock? ReceivingStock { get; set; }
        public Prescription? Prescription { get; set; }
        public ConcoctionLine? ConcoctionLine { get; set; }
        public TransferStock? TransferStock { get; set; }
        public Product? Product { get; set; }
        public Location? Source {  get; set; }
        public Location? Destination { get; set; }

    }
}
