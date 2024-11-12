using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class WellnessProgramSession : BaseAuditableEntity
    {
        public long WellnessProgramId { get; set; } 
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Time { get; set; }
        public string? Day { get; set; }
        public string? Description { get; set; }

        public WellnessProgram? WellnessProgram { get; set; }   
    }
}
