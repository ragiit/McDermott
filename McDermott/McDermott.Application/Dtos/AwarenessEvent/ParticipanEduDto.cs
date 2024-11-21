using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.AwarenessEvent
{
    public class ParticipanEduDto : IMapFrom<ParticipanEdu>
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