using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanCPPTDto : IMapFrom<GeneralConsultanCPPT>
    {
        public int Id { get; set; }
        public int GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}
