namespace McDermott.Application.Dtos.Config
{
    public class EmailSettingDto : IMapFrom<EmailSetting>
    {
        public long Id { get; set; }

        [StringLength(300)]
        public string? Description { get; set; } = string.Empty;

        public long? Sequence { get; set; }
        public bool? Smtp_Debug { get; set; }

        [Required(ErrorMessage = "Connection Security Must Be Filled In!")]
        public string? Smtp_Encryption { get; set; }

        public string? Status { get; set; } = string.Empty;

        [StringLength(200)]
        [Required(ErrorMessage = ("SMTP Server Must Be Filled In!"))]
        public string Smtp_Host { get; set; } = string.Empty;

        [Required(ErrorMessage = ("Password Must Be Filled In!"))]
        public string? Smtp_Pass { get; set; } = string.Empty;

        [Required(ErrorMessage = ("SMTP Port Must Be Filled In!"))]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? Smtp_Port { get; set; } = string.Empty;

        [Required(ErrorMessage = ("Username Must Be Filled In!"))]
        [StringLength(200)]
        public string? Smtp_User { get; set; } = string.Empty;
    }
}