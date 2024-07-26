namespace McHealthCare.Domain.Entities
{
    public class Pharmacy : BaseAuditableEntity
    {
        public Guid? PatientId { get; set; }
        public Guid? PractitionerId { get; set; }
        public Guid? PrescriptionLocationId { get; set; }
        public Guid? MedicamentGroupId { get; set; }
        public Guid? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public EnumStatusPharmacy? Status { get; set; }

        public Location? Location { get; set; }

        public MedicamentGroup? MedicamentGroup { get; set; }

        public Service? Service { get; set; }

        // public User? Patient { get; set; }

        // public User? Practitioner { get; set; }

        public virtual List<Prescription>? Prescriptions { get; set; }
    }
}