using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanMedicalSupportLogDto :IMapFrom<GeneralConsultanMedicalSupportLog>
    {
        public long Id { get; set; }
        public long? GeneralConsultationMedicalSupportId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantServiceProcedureRoom Status { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [SetToNull]
        public GeneralConsultanMedicalSupportDto? GeneralConsultanMedicalSupport { get; set; }
        [SetToNull]
        public UserDto? UserBy { get; set; }
    }
}
