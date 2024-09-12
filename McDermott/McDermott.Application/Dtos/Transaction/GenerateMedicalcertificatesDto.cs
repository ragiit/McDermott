using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GenerateMedicalcertificatesDto
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? PatientId { get; set; }

        public virtual UserDto? Patient { get; set; }
        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}
