using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class MedicamentGroup : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public bool? IsConcoction { get; set; }
        public Guid? PhycisianId { get; set; }
        public Guid? UoMId { get; set; }
        public Guid? FormDrugId { get; set; }

        // public virtual User? Phycisian { get; set; }

        public virtual Uom? UoM { get; set; }

        public virtual DrugForm? FormDrug { get; set; }
    }
}