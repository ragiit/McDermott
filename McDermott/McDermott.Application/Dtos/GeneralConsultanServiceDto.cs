using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? BirthDay { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? DateSchendule { get; set; } = DateTime.Now;
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }

        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? Pratitioner { get; set; }
        public virtual InsuranceDto? Insurance { get; set; }
        public virtual ServiceDto? Service { get; set; }
    }
}
