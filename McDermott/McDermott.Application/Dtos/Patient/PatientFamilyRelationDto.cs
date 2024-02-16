using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Patient
{
    public class PatientFamilyRelationDto : IMapFrom<PatientFamilyRelation>
    {
        public int Id { get; set; } 
        public int PatientId { get; set; }
        public int FamilyMemberId { get; set; } 
        public int? FamilyId { get; set; }
        public string? Relation { get; set; }

        public virtual FamilyDto? Family { get; set; }
        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? FamilyMember {  get; set; }    
    }
}
