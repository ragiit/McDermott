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

        public string? Status { get; set; } = string.Empty;

        [StringLength(200)]
        public string? To { get; set; } = string.Empty;

        public string? Cc { get; set; } = string.Empty;
        public string? ReplayTo { get; set; } = string.Empty;
        public DateTime? Schendule { get; set; }
        public string? Message { get; set; }
    }
}