using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class WellnessProgramParticipant : BaseAuditableEntity
    {
        public long WellnessProgramId { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }

        public WellnessProgram? WellnessProgram { get; set; }
        public User? Patient { get; set; }
    }
}