namespace McHealthCare.Domain.Entities
{
    public partial class EmailSetting : BaseAuditableEntity
    {
        [StringLength(300)]
        public string? Description { get; set; }

        public Guid? Sequence { get; set; }
        public bool? Smpt_Debug { get; set; }
        public string? Smtp_Encryption { get; set; }

        [StringLength(200)]
        public string? Smtp_Host { get; set; }

        public string? Smtp_Pass { get; set; }
        public string? Status { get; set; }

        public string? Smtp_Port { get; set; }

        [StringLength(200)]
        public string? Smtp_User { get; set; }
    }
}