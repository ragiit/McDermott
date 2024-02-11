using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public int PatientId { get; set; }  
        public string? Farmacology { get; set; }
        public string? Weather { get; set; }
        public string? Food { get; set; }

        public virtual User? User { get; set; }
    }
}
