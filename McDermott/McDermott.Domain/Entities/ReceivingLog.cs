using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ReceivingLog :BaseAuditableEntity
    {
        public long? ReceivingId { get; set; }
        public long? SourceId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }

        public ReceivingStock? Receiving {  get; set; }
        public User? UserBy { get; set; }
        public Locations? Source { get; set; }
    }
}
