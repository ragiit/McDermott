using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities.Configuration
{
    public class Patient
    {
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }  // Primary key and foreign key

        public string? NoRm { get; set; } = "-";
        public string? IsFamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string? IsMedicationHistory { get; set; }
        public string? MedicationHistory { get; set; }
        public string? PastMedicalHistory { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }  // Navigation property
    }
}