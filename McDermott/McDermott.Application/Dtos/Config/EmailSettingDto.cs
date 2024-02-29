using System.ComponentModel.DataAnnotations;


namespace McDermott.Application.Dtos.Config
{
    public class EmailSettingDto : IMapFrom<EmailSetting>
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string? Description { get; set; } = string.Empty;
        public int? Sequence { get; set; }
        public bool? Smtp_Debug { get; set; }
        public string? Smtp_Encryption { get; set; }=string.Empty;
        public string? Status { get; set;} = string.Empty;

        [StringLength(200)]
        public string Smtp_Host { get; set; } = string.Empty;
        public string? Smtp_Pass { get; set; } = string.Empty;
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? Smtp_Port { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Smtp_User { get; set; } = string.Empty;

    }
}
