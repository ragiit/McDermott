using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class Menu : BaseAuditableEntity
    {
        public Guid? ParentId { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; 
        public string? Icon { get; set; }
        public long Sequence { get; set; } = 0;
        public string? Url { get; set; }

        public Menu? Parent {  get; set; }  
    }
}
