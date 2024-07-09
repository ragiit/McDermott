using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class StockOutLines:BaseAuditableEntity
    {
        public long? LinesId { get; set; }
        public long? StockId { get; set; }
        public long? CutStock { get; set; }

        [SetToNull]
        public ConcoctionLine? Lines { get; set; }
        [SetToNull]
        public StockProduct? Stock { get; set; }
    }
}
