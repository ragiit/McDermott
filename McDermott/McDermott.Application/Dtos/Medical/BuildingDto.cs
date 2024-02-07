using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Medical
{
    public class BuildingDto : IMapFrom<Building>
    {
        public int Id { get; set; }
        public int? HealthCenterId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Code { get; set; } = string.Empty;

        public HealthCenterDto? HealthCenter { get; set; }
    }
}