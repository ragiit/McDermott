using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultationLog : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? ProcedureRoomId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantService? Status { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public GeneralConsultanMedicalSupport? ProcedureRoom { get; set; }
        public User? UserBy {  get; set; }
        
    }
}
