using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class WellnessProgram : BaseAuditableEntity
    {
        public string? Category { get; set; } // Id
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public EnumWellness Status { get; set; } = EnumWellness.Draft;

        public List<WellnessProgramDetail>? WellnessProgramDetails { get; set; }
    }
}