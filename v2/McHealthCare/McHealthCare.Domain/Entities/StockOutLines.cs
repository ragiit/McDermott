using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class StockOutLines:BaseAuditableEntity
    {
        public Guid? LinesId { get; set; }
        public Guid? TransactionStockId { get; set; }
        public Guid? CutStock { get; set; }

        
        public ConcoctionLine? Lines { get; set; }
        
        public TransactionStock? TransactionStock { get; set; }
    }
}
