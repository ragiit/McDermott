namespace McDermott.Domain.Entities
{
    public class Pharmacy : BaseAuditableEntity
    {
        public long? PatientId { get; set; }
        public long? PractitionerId { get; set; }
        public long? PrescriptionLocationId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public EnumStatusPharmacy? Status { get; set; }

        [SetToNull]
        public Locations? Location { get; set; }

        [SetToNull]
        public MedicamentGroup? MedicamentGroup { get; set; }

        [SetToNull]
        public Service? Service { get; set; }

        [SetToNull]
        public User? Patient { get; set; }

        [SetToNull]
        public User? Practitioner { get; set; }

        [SetToNull]
        public virtual List<Prescription>? Prescriptions { get; set; }
    }
}