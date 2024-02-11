using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Patient
{
    public class PatientAllergyDto : IMapFrom<PatientAllergy>
    {
        public int PatientId { get; set; }
        public string? Farmacology { get; set; }
        public string? Weather { get; set; }
        public string? Food { get; set; }

        public UserDto? User { get; set; }
    }
}
