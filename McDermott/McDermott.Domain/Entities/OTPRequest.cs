using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    [Table("OTPRequests", Schema = "Telemedicine")]
    public partial class OTPRequest : BaseAuditableEntity
    {
        public long UserId { get; set; }
        public string OTPCode { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public bool IsVerified { get; set; } = false;
        public short AttemptCount { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime? RetryAfter { get; set; }

        public virtual User? User { get; set; }
    }
}