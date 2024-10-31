using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultationServiceLogDto :IMapFrom<GeneralConsultationServiceLog>
    {
        public long Id { get; set; }
        public long? GeneralConsultationServiceId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantService Status { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [SetToNull]
        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        [SetToNull]
        public UserDto? UserBy { get; set; }
    }
}
