using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class AccidentDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Date Of Occurrence")]
        public DateTime? DateOfOccurrence { get; set; }
        public DateTime? DateOfFirstTreatment { get; set; }
        public string? AreaOfYard { get; set; }
        public bool RibbonSpecialCase { get; set; } = false;
        public string? NatureOfInjury { get; set; }
        public string? PartOfBody { get; set; }
        public string? CaouseOfInjury { get; set; }
        public string? Class { get; set; }
        public string? Sent { get; set; }
        public string? Treatment { get; set; }

        public virtual UserDto? Employee { get; set; }
    }
}
