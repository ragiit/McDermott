using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class ReportDto
    {
        public string? report { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = DateTime.Now;
    }

    public class VisitByPeriod
    {
        public int? TotalVisit { get; set; }
        public string? Services { get; set; }
        public int? CountPatient { get; set; }
    }
}
