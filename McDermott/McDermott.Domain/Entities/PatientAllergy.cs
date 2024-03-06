namespace McDermott.Domain.Entities
{
    public class PatientAllergy : BaseAuditableEntity
    {
        public long UserId { get; set; }
        public string? Farmacology { get; set; }
        public string? Weather { get; set; }
        public string? Food { get; set; }

        [SetToNull]
        public virtual User? User { get; set; }
    }
}