namespace McHealthCare.Domain.Entities
{
    public partial class EmailTemplate : BaseAuditableEntity
    {
        [StringLength(200)]
        public string? Subject { get; set; }

        [StringLength(200)]
        public string? From { get; set; }

        public Guid? ById { get; set; }

        [StringLength(200)]
        public string? To { get; set; }

        public Guid? ToPartnerId { get; set; }

        [StringLength(200)]
        public string? Cc { get; set; }

        public string? ReplayTo { get; set; }
        public DateTime? Schendule { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }

        //public virtual User? By { get; set; }

        //public virtual List<User>? ToPartner { get; set; }
    }
}