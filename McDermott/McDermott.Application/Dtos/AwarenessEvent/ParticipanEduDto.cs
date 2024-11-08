using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class ParticipanEduDto:IMapFrom<ParticipanEdu>
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public long? EducationProgramId { get; set; }
        public DateTime? CreatedDate { get; set; }

        [SetToNull]
        public UserDto? Patient { get; set; }
        [SetToNull]
        public EducationProgramDto? EducationProgram { get; set; }
    }
}
