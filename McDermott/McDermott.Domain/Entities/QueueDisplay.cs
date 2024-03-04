using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class QueueDisplay:BaseAuditableEntity
    {
        public string? Name {  get; set; }
        public List<int>? CounterId { get; set; }

        public virtual List<Counter>? Counter {  get; set; }
    }
}
