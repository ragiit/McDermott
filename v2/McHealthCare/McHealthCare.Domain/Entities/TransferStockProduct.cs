using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class TransferStockProduct : BaseAuditableEntity
    {
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Guid? TransferStockId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? QtyStock { get; set; }

        public TransferStock? TransferStock { get; set; }
        public Product? Product { get; set; }
    }
}