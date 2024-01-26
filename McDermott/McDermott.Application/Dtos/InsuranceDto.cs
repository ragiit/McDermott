using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class InsuranceDto : IMapFrom<Insurance>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public bool? IsBPJS { get; set; } = false;
        public int? AdminFee { get; set; }
        public int? Presentase { get; set; }
        public int? AdminFeeMax { get; set; }
    }
}