using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStockDetail : BaseAuditableEntity
    {
        public long? TransactionStockId { get; set; }
        public long? StockId { get; set; }
        public long? ProductId { get; set; }
        public long? QtyStock { get; set; }
        public string? StatusStock { get; set; }

        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }
        [SetToNull]
        public StockProduct? Stock { get; set; }
        [SetToNull]
        public Product? Product { get; set; }
    }
}