using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramDetailDto : IMapFrom<WellnessProgramDetail>
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Slug { get; set; }

        public WellnessProgramDto? WellnessProgram { get; set; }
    }

    public class CreateUpdateWellnessProgramDetailDto
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Slug { get; set; }
    }
}