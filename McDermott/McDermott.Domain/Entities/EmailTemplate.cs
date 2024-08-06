namespace McDermott.Domain.Entities
{
    public partial class EmailTemplate : BaseAuditableEntity
    {
        [StringLength(200)]
        public string? Subject { get; set; }

        [StringLength(200)]
        public string? From { get; set; }
        public long? EmailFromId { get; set; }
        public long? ById { get; set; }

        [StringLength(200)]
        public string? To { get; set; }

        public long? ToPartnerId { get; set; }

        [StringLength(200)]
        public List<string>? Cc { get; set; } = [];

        public string? ReplayTo { get; set; }
        public DateTime? Schendule { get; set; }
        public byte[]? DocumentContent { get; set; } = [];
        public string? Message { get; set; }
        public string? Status { get; set; }

        [SetToNull]
        public virtual User? By { get; set; }

        [SetToNull]
        public virtual List<User>? ToPartner { get; set; }
        public virtual EmailSetting? EmailFrom {  get; set; }
    }
}