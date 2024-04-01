using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class Medicament :BaseAuditableEntity
    {
        public string? Name { get; set; }
        public int? PhycisianId { get; set; }
    }
}
