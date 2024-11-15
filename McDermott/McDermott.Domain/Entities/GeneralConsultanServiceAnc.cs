using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanServiceAnc : BaseAuditableEntity
    { 
        public long GeneralConsultanServiceId {  get; set; } 
        public long PatientId {  get; set; } 
        public string Reference { get; set; } = string.Empty;
        public string? PregnancyStatusG { get; set; }
        public string? PregnancyStatusP { get; set; }
        public string? PregnancyStatusA { get; set; }
        public DateTime? HPHT { get; set; }
        public DateTime? HPL { get; set; }
        public string? LILA { get; set; }  

        public GeneralConsultanService? GeneralConsultanService { get; set; }
        public User? Patient { get; set; }
    }
}
