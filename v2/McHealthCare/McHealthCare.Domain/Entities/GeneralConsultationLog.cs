using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class GeneralConsultationLog : BaseAuditableEntity
    {
        public Guid? GeneralConsultanServiceId { get; set; }
        public Guid? ProcedureRoomId { get; set; }
        public Guid? UserById { get; set; }
        public string? Status { get; set; }

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public GeneralConsultanMedicalSupport? ProcedureRoom { get; set; }
        //public User? UserBy {  get; set; }
    }
}