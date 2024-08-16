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

    public class CreateUpdatePatientDto
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