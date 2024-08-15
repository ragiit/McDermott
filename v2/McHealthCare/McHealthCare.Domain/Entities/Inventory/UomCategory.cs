using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Inventory
{
    public class UomCategory : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
    }
}
