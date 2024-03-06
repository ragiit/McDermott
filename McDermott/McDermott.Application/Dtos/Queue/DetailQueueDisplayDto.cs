using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Queue
{
    public class DetailQueueDisplayDto:IMapFrom<DetailQueueDisplay>
    {
        public int Id { get; set; }
        public int? QueueDisplayId { get; set; }
        public int? CounterId { get; set; }
        public string? CounterName {  get; set; } = string.Empty;
        public string? DisplayName {  get; set; } = string.Empty;

        public virtual QueueDisplay? QueueDisplay { get; set; }
        public virtual Counter? Counter { get; set; }
    }
}
