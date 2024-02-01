using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanService : BaseAuditableEntity
    {
        public int? PatientId { get; set; }
        public int? InsuranceId { get; set; }
        public int? ServiceId { get; set; }
        public int? PratitionerId { get; set; }
        public int? ClassType { get; set; }
        public string? NoRM { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? TypeRegistration { get; set; }
        public DateTime? DateSchendule { get; set; }
        public TimeSpan? TimeSchendule { get; set; }

        public virtual User? Patient { get; set; }
        public virtual User? Pratitioner { get; set; }
        public virtual Insurance? Insurance { get; set; }
        public virtual Service? Service { get; set; }


    }
}
