using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    [Table("TelemedicineConsultationCPPT", Schema = "Telemedicine")]
    public partial class TelemedicineConsultationCPPT : BaseAuditableEntity
    {
        public long TelemedicineConsultationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public long? UserId { get; set; }
        public string? Subjective { get; set; }
        public string? Objective { get; set; }
        public long? DiagnosisId { get; set; }
        public long? NursingDiagnosesId { get; set; }
        public string? Planning { get; set; }
        public string? Anamnesa { get; set; }
        public string? MedicationTherapy { get; set; }
        public string? NonMedicationTherapy { get; set; }

        public User? User { get; set; }
        public Diagnosis? Diagnosis { get; set; }
        public NursingDiagnoses? NursingDiagnoses { get; set; }
        public virtual TelemedicineConsultation? TelemedicineConsultation { get; set; }
    }
}