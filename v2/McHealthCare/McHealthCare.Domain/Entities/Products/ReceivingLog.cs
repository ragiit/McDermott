using McHealthCare.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class ReceivingLog :BaseAuditableEntity
    {
        public Guid? ReceivingId { get; set; }
        public Guid? SourceId { get; set; }
        public string? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }

        public ReceivingStock? Receiving {  get; set; }
        public ApplicationUser? UserBy { get; set; }
        public Location? Source { get; set; }
    }
}
