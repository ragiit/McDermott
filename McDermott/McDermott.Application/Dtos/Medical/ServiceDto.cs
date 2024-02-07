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
    }
}