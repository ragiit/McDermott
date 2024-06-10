using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class PharmacyLogDto : IMapFrom<PharmacyLog>
    {
        public long Id { get; set; }

        public long? PharmacyId { get; set; }
        public long? UserById { get; set; }
        public string? status { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public PharmacyDto? Pharmacy { get; set; }
        public UserDto? UserBy { get; set; }

    }
}
