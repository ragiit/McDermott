using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class TempDetailQueueDisplayDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? QueueDisplayId { get; set; }
        public int? CounterId { get; set; }
        public string? NameCounter { get; set; }
    }
}
