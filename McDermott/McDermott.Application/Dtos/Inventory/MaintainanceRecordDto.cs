using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public partial class MaintainanceRecordDto : IMapFrom<MaintainanceRecord>
    {
        public long Id { get; set; }
        public long? MaintainanceId { get; set; }
        public long? ProductId { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentName { get; set; }
        public string? SequenceProduct { get; set; }

        [SetToNull]
        public virtual MaintainanceDto? Maintainance { get; set; }
        [SetToNull]
        public virtual ProductDto? Product { get; set; }
    }
}
