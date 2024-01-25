using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class DiseaseCategory: BaseAuditableEntity
    {
        [StringLength(300)]
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }

        public virtual List<ParentCategory>? ParentCategory { get; set; }
    }
}
