namespace McDermott.Application.Dtos.Config
{
    public class EmailSettingDto : IMapFrom<EmailSetting>
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string? Description { get; set; } = string.Empty;

        public int? Sequence { get; set; }
        public bool? Smtp_Debug { get; set; }

        [Required(ErrorMessage = "Connection Scurity must be filled in!")]
        public string? Smtp_Encryption { get; set; }

        public string? Status { get; set; } = string.Empty;

        [StringLength(200)]
        [Required(ErrorMessage = ("SMTP Server must be filled in!"))]
        public string Smtp_Host { get; set; } = string.Empty;

        [Required(ErrorMessage = ("Password must be filled in!"))]
        public string? Smtp_Pass { get; set; } = string.Empty;

        [Required(ErrorMessage = ("SMTP Port must be filled in!"))]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? Smtp_Port { get; set; } = string.Empty;

        [Required(ErrorMessage = ("Username must be filled in!"))]
        [StringLength(200)]
        public string? Smtp_User { get; set; } = string.Empty;
    }
}