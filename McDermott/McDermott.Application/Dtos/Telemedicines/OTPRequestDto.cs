using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Telemedicines
{
    public class OTPRequestDto : IMapFrom<OTPRequest>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OTPCode { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public bool IsVerified { get; set; } = false;
        public short AttemptCount { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime? RetryAfter { get; set; }

        public virtual UserDto? User { get; set; }
    }

    public class CreateUpdateOTPRequestDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OTPCode { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public bool IsVerified { get; set; } = false;
        public short AttemptCount { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime? RetryAfter { get; set; }
    }
}