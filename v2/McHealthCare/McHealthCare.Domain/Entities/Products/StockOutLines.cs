using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class StockOutLines:BaseAuditableEntity
    {
        public Guid? LinesId { get; set; }
        public Guid? TransactionStockId { get; set; }
        public long? CutStock { get; set; }

        [SetToNull]
        public ConcoctionLine? Lines { get; set; }
        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }
    }
}
