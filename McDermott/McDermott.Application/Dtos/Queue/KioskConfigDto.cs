using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Queue
{
    public class KioskConfigDto: IMapFrom<KioskConfig>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<int>? ServiceIds { get; set; } = [];
        public string ServiceName { get; set; }=string.Empty;
        public virtual ServiceDto? Service { get; set; }
    }
}
