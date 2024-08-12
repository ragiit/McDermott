using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class EmailSettingDto : IMapFrom<EmailSetting>
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }

        public long? Sequence { get; set; }
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

    public class CreateUpdateEmailSettingDto
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string? Description { get; set; } 
        public long? Sequence { get; set; }
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
