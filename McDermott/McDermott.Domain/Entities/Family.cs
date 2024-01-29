using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Family:BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)] 
        public string? Relation { get; set; }
    }
}
