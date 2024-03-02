namespace McDermott.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public string? Farmacology { get; set; }
        public string? Weather { get; set; }
        public string? Food { get; set; }

        public virtual User? User { get; set; }
    }
}