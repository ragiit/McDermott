namespace McHealthCare.Domain.Entities.Configuration
{
    public class EmailTemplate : BaseAuditableEntity
    {
        [StringLength(200)]
        public string? Subject { get; set; }

        [StringLength(200)]
        public string? From { get; set; }

        public Guid? EmailFromId { get; set; }
        public string? ById { get; set; }

        [StringLength(200)]
        public string? To { get; set; }

        public string? ToPartnerId { get; set; }

        [StringLength(200)]
        public List<string>? Cc { get; set; } = [];

        public string? ReplayTo { get; set; }
        public DateTime? Schendule { get; set; }
        public byte[]? DocumentContent { get; set; } = [];
        public string? Message { get; set; }
        public long? TypeEmail { get; set; }
        public string? Status { get; set; }

        public virtual ApplicationUser? By { get; set; }
        public virtual List<ApplicationUser>? ToPartner { get; set; }
        public virtual EmailSetting? EmailFrom { get; set; }
    }
}