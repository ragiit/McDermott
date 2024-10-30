using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultationServiceLog : BaseAuditableEntity
    {
        public long? GeneralConsultationServiceId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGeneralConsultantService status {get;set;}

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public User? UserBy {  get; set; }

    }
}
