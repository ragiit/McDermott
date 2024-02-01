using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public partial class GeneralConsultanServiceDto:IMapFrom<GeneralConsultanService>
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? InsuranceId { get; set; }
        public int? ServiceId { get; set; }
        public int? PratitionerId { get; set; }
        public int? ClassType { get; set; }
        public string? NoRM { get; set; } = string.Empty;
        public string? IdentityNumber { get; set; } = string.Empty;
        public string? TypeRegistration { get; set; } = string.Empty;
        public DateTime? DateSchendule { get; set; }
        public TimeSpan? TimeSchendule { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
