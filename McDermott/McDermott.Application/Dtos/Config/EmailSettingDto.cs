using System.ComponentModel.DataAnnotations;


namespace McDermott.Application.Dtos.Config
{
    public class EmailSettingDto : IMapFrom<EmailSetting>
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string? Description { get; set; } = string.Empty;
        public int? Sequence { get; set; }
        public bool? Smpt_Debug { get; set; }
        public string? Smpt_Encryption { get; set; }

        [StringLength(200)]
        public string? Smpt_Host { get; set; } = string.Empty;
        public string? Smpt_Pass { get; set; } = string.Empty;
        public int? Smpt_Port { get; set; }

        [StringLength(200)]
        public string? Smpt_User { get; set; } = string.Empty;

    }
}
