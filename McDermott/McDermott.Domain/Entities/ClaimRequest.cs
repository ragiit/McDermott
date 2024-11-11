using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class ClaimRequest:BaseAuditableEntity
    {
       
        public int ClaimRequestId { get; set; }

        public long? PatientId { get; set; }
        public User? Patient { get; set; } // Navigation property untuk data pasien

        public long? PhycisianId { get; set; }
        public User? Phycisian { get; set; } // Navigation property untuk data dokter

        public DateTime ClaimDate { get; set; }

        public long? BenefitId { get; set; }
        public BenefitConfiguration? Benefit { get; set; } // Navigation property untuk data benefit

        public string? Remark { get; set; } // Field catatan atau detail klaim

        public EnumClaimRequestStatus? Status { get; set; } // Enum untuk 'Draft' dan 'Done'

        public bool? IsNotificationSent { get; set; } // Status notifikasi otomatis

    }
}
