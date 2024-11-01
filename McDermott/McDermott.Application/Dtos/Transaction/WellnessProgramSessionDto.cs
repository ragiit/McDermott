using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramSessionDto : IMapFrom<WellnessProgramSession>
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now; 
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(7);
        public string? Time { get; set; }
        public string? Day { get; set; }
        public string? Description { get; set; }

        public WellnessProgramDto? WellnessProgram { get; set; }
    }

    public class CreateUpdateWellnessProgramSessionDto 
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(7);
        public string? Time { get; set; }
        public string? Day { get; set; }
        public string? Description { get; set; } 
    }
}
