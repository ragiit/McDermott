namespace McDermott.Application.Dtos.Config
{
    public class EmailTemplateDto : IMapFrom<EmailTemplate>
    {
        public long Id { get; set; }
        public long? ById { get; set; }
        public long? ToPartnerId { get; set; }

        [StringLength(200)]
        public string? Subject { get; set; } = string.Empty;

        [StringLength(200)]
        public string? From { get; set; } = string.Empty;
        public long? EmailFromId { get; set; }

        public string? Status { get; set; } = string.Empty;

        [StringLength(200)]
        public string? To { get; set; } = string.Empty;

        public List<string>? Cc { get; set; } = [];
        public string? ReplayTo { get; set; } = string.Empty;

        public long? TypeEmail { get; set; }
        public DateTime? Schendule { get; set; }
        //public byte[]? DocumentContent { get; set; } = [];
        //public string? Message => System.Text.Encoding.UTF8.GetString(DocumentContent ?? []);
        public string? Message { get;set; } = string.Empty;

        public virtual EmailSettingDto? EmailFrom { get; set; }
    }
}