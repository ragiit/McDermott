using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Medical
{
    public class ServiceDto : IMapFrom<Service>
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; } = false;
        public bool IsKiosk { get; set; } = false;
        public string Flag { get; set; } = string.Empty;
        public string KioskName {  get; set; } = string.Empty;
        public int ServicedId { get; set; } 
        public virtual ServiceDto? Service { get; set; }
    }
}