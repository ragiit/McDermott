using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ParticipanAwareness:BaseAuditableEntity
    {
        public long? PatientId { get; set; }
        public long? AwarenessId { get; set; }

        [SetToNull]
        public virtual User? Patient { get; set; }
    }
}
