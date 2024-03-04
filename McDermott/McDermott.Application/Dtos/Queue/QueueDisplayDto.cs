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
        public int? CounterId { get; set; }
    }
}
