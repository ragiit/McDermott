using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramParticipantDto : IMapFrom<WellnessProgramParticipant>
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long WellnessProgramDetailId { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }

        public WellnessProgramDto? WellnessProgram { get; set; }
        public UserDto? Patient { get; set; }
    }

    public class CreateUpdateWellnessProgramParticipantDto
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long WellnessProgramDetailId { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }
    }
}