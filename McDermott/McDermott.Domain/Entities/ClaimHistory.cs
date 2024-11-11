using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ClaimHistory:BaseAuditableEntity
    {
     
        public long? BenefitId { get; set; }
        public BenefitConfiguration? Benefit { get; set; } // Navigation property

        public long? PatientId { get; set; }
        public User? Patient { get; set; } // Navigation property

        public DateTime ClaimDate { get; set; }

       
        public int ClaimedValue { get; set; } // Bisa berupa qty atau amount sesuai dengan konfigurasi benefit
    }
}
