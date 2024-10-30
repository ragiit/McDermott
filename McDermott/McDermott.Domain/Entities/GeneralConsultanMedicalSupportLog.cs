using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultanMedicalSupportLog :BaseAuditableEntity
    {
        public long? GeneralConsultationMedicalSupportId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantServiceProcedureRoom Status { get; set; }
        public GeneralConsultanMedicalSupport? GeneralConsultanMedicalSupport { get; set; }
        public User? UserBy {  get; set; }

    }
}
