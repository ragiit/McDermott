using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Counter:BaseAuditableEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
