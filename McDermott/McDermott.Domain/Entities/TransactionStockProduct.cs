using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class TransactionStockProduct : BaseAuditableEntity
    {
        public long? StockProductId { get; set; }
        public long? TransactionStockId { get; set; }
        public long? ProductId { get; set; }
        public long? QtyStock { get; set; }

        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }

        [SetToNull]
        public Product? Product { get; set; }

        [SetToNull]
        public StockProduct? StockProduct { get; set; }
    }
}