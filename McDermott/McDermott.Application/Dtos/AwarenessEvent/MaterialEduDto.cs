using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class MaterialEduDto:IMapFrom<MaterialEdu>
    {
        [Required(ErrorMessage = "Material Content is required.")]
        public string MaterialContent { get; set; } = string.Empty;
        public string? Attendance { get; set; }
        public long? EducationProgramId { get; set; }

        [SetToNull]
        public  EducationProgramDto? EducationProgram { get; set; }
    }
}
