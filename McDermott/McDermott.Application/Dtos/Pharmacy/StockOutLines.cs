using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class StockOutLines: IMapFrom<StockOutLines>
    {
        public long? LinesId { get; set; }
        public long? StockId { get; set; }
        public long? CutStock { get; set; }
        public long? CurrentStock { get; set; }

        [SetToNull]
       public virtual ConcoctionLineDto? Lines { get; set; }
        [SetToNull]
        public virtual StockProductDto? Stock { get; set; }
    }
}
