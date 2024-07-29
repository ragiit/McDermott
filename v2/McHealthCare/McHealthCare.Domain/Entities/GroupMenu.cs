using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class GroupMenu : BaseAuditableEntity
    { 
        public Guid GroupId { get; set; }
        public Guid MenuId { get; set; }

        public Group Group { get; set; } = new();
        public Menu Menu { get; set; } = new();
    }
}
