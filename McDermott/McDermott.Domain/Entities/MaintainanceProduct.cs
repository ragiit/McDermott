using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MaintainanceProduct : BaseAuditableEntity
    {
        public long? MaintainanceId { get; set; }
        public long? ProductId { get; set; }
        public string? SerialNumber { get; set; }
        public string? Note { get; set; }
        public DateTime? Expired {  get; set; }
        public EnumStatusMaintainance? Status { get; set; }

        [SetToNull]
        public Maintainance? Maintainance { get; set; }
        [SetToNull]
        public Product? Product { get; set; }

    }
}
