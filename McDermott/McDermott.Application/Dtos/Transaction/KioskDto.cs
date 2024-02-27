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
        public string? Type { get; set; }
        [Required]
        [StringLength(150)]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NumberType { get; set; } = string.Empty;
        public int? PatientId { get; set; }
        public string? BPJS { get; set; } = string.Empty;
        public bool? StageBpjs { get; set; }
        public int? ServiceId { get; set; }
        public int? PhysicianId { get; set; }
        public int? Queue { get; set; }
        public int? CounterId { get; set; }
        public  virtual CounterDto? Counter  { get; set; }
        public virtual UserDto? Patient { get; set; }
        
    }
}
