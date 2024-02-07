using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Config
{
    public class EmailTemplateDto : IMapFrom<EmailTemplate>
    {
        public int id { get; set; }
        public int? ById { get; set; }
        public int? ToPartnerId { get; set; }

        [StringLength(200)]
        public string? Subject { get; set; } = string.Empty;

        [StringLength(200)]
        public string? From { get; set; } = string.Empty;

        [StringLength(200)]
        public string? To { get; set; } = string.Empty;
        public string? Cc { get; set; } = string.Empty;
        public string? ReplayTo { get; set; } = string.Empty;
        public DateTime? Schendule { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
