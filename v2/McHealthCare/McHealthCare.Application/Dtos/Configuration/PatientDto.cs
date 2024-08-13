using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class PatientDto : IMapFrom<Patient>
    { 
        public string? ApplicationUserId { get; set; }    
        public string? NoRm { get; set; } = "-";
        public string? IsFamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string? IsMedicationHistory { get; set; }
        public string? MedicationHistory { get; set; }
        public string? PastMedicalHistory { get; set; }
    }
}
