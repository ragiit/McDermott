namespace McHealthCare.Application.Dtos.Configuration

{
    public class EmailTemplateDto : IMapFrom<EmailTemplate>
    {
        public Guid Id { get; set; }
        public string? ById { get; set; }
        public string? ToPartnerId { get; set; }

        [StringLength(200)]
        public string? Subject { get; set; } = string.Empty;

        [StringLength(200)]
        public string? From { get; set; } = string.Empty;

        public Guid? EmailFromId { get; set; }

        public string? Status { get; set; } = string.Empty;

        [StringLength(200)]
        public string? To { get; set; } = string.Empty;

        public List<string>? Cc { get; set; } = [];
        public string? ReplayTo { get; set; } = string.Empty;

        public long? TypeEmail { get; set; }
        public DateTime? Schendule { get; set; }

        //public byte[]? DocumentContent { get; set; } = [];
        //public string? Message => System.Text.Encoding.UTF8.GetString(DocumentContent ?? []);
        public string? Message { get; set; } = string.Empty;

        public virtual EmailSettingDto? EmailFrom { get; set; }
    }

    public class CreateUpdateEmailTemplateDto : IMapFrom<EmailTemplate>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Subject { get; set; }

        [StringLength(200)]
        public string? From { get; set; }

        public Guid? EmailFromId { get; set; }
        public Guid? ById { get; set; }

        [StringLength(200)]
        public string? To { get; set; }

        public Guid? ToPartnerId { get; set; }

        [StringLength(200)]
        public List<string>? Cc { get; set; } = [];

        public string? ReplayTo { get; set; }
        public DateTime? Schendule { get; set; }
        public byte[]? DocumentContent { get; set; } = [];
        public string? Message { get; set; }
        public long? TypeEmail { get; set; }
        public string? Status { get; set; }
    }
}