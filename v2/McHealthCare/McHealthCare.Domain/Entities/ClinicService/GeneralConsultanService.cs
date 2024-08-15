using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.ClinicService
{
    public class GeneralConsultanService : BaseAuditableEntity
    {
        public Guid? ServiceId {  get; set; }   
        public virtual Service? Service { get; set; }
    }
}
