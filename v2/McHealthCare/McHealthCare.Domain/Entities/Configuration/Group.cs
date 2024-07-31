using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class Group : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}
