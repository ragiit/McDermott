using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class StockOutLinesDto : IMapFrom<StockOutLines>
    {
        public long Id { get; set; }
        public long? LinesId { get; set; }
        public long? StockId { get; set; }
        public long CutStock { get; set; } = 0;
        public long? CurrentStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? Expired { get; set; }

        [SetToNull]
        public virtual ConcoctionLineDto? Lines { get; set; }
        [SetToNull]
        public virtual StockProductDto? Stock { get; set; }
    }
}
