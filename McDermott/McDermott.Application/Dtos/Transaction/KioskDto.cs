using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public partial class KioskDto : IMapFrom<Kiosk>
    {
        public int Id { get; set; }
        public string? Type { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public string? NumberType { get; set; } = string.Empty;
        public int? PatientId { get; set; }
        public string? Insurance { get; set; } = string.Empty;
        public bool? StageInsurance { get; set; }
        public int? ServiceId { get; set; }
        public int? PhysicianId { get; set; }
    }
}
