namespace McDermott.Application.Dtos.Pharmacy
{
    public class PharmacyDto
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public long? PractitionerId { get; set; }
        public long? PrescriptionLocationId { get; set; }
        public long? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public EnumStatusPharmacy Status { get; set; }

        [NotMapped]
        public string StatusText
        {
            get { return Status.EnumGetValue<string>(); }
        }

        public LocationDto? Location { get; set; }
        public ServiceDto? Service { get; set; }
        public UserDto? Patient { get; set; }
        public UserDto? Practitioner { get; set; }
    }
}
