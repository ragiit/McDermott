using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MaintainanceRecord : BaseAuditableEntity
    {
        public long? MaintainanceId { get; set; }
        public long? ProductId { get; set; }
        public string? DocumentBase64 { get; set; }
        public string? DocumentName { get; set; }
        public string? SequenceProduct { get; set; }

        [SetToNull]
        public virtual Maintainance? Maintainance { get; set; }
        [SetToNull]
        public virtual Product? Product { get; set; }

    }
}
