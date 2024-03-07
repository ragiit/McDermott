using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class DetailQueueDisplay: BaseAuditableEntity
    {
        public long? QueueDisplayId { get; set; }
        public long? CounterId { get; set; }

        public virtual QueueDisplay? QueueDisplay { get; set; }
        public virtual Counter? Counter { get; set; }
    }
}
