using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Queue
{
    public class KioskQueueDto:IMapFrom<KioskQueue>
    {
        public int Id { get; set; }
        public int? KioskId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? NoQueue {  get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Kiosk? Kiosk { get; set; }
    }
}
