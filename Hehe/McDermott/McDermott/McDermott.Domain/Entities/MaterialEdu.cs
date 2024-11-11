using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MaterialEdu:BaseAuditableEntity
    {
        public string? MaterialContent { get; set; }
        public string? Attendance {  get; set; }
        public long? EducationProgramId { get; set; }

        [SetToNull]
        public virtual EducationProgram? EducationProgram { get; set; }
    }
}
