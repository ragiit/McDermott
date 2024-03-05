using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Queue
{
    public class QueueDisplayDto:IMapFrom<QueueDisplay>
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? NameCounter { get; set; }=string.Empty;
        public int? listIdCounter { get; set; }
        public List<int>? CounterId { get; set; } = [];
        public virtual List<CounterDto>? Counter {  get; set; }
    }
}
