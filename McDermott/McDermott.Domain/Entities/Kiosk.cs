using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Kiosk :BaseAuditableEntity
    {
        public string? Type {  get; set; }
        public string? NumberType { get; set; }
        public int? PatientId { get; set; }
        public string? Insurance {  get; set; }
        public bool? StageInsurance { get; set; }
        public int? ServiceId { get; set; }
        public int? PhysicianId {  get; set; }

        public virtual Service? Service { get; set; }
        public virtual User? Patient { get; set; }
        public virtual User? Physician {  get; set; }

    }
}
