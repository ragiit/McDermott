using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class ReceivingLog : BaseAuditableEntity
    {
        public Guid? ReceivingId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }

        public ReceivingStock? Receiving { get; set; }

        //public User? UserBy { get; set; }
        public Location? Source { get; set; }
    }
}