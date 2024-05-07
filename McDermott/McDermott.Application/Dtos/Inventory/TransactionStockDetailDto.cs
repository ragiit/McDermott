using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransactionStockDetailDto : IMapFrom<TransactionStockDetail>
    {
        public long Id { get; set; }
        public long? TransactionStockId { get; set; }
        public long? StockId { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public long? ProductId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long? QtyStock { get; set; }

        public string? StatusStock { get; set; }

        public string? ProductName { get; set; }
        public string? UomName { get; set; }

        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }
        [SetToNull]
        public virtual StockProductDto? Stock { get; set; }
        [SetToNull]
        public virtual ProductDto? Product { get; set; }
    }
}