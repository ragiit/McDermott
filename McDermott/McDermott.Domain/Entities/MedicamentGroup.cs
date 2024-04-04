using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MedicamentGroup :BaseAuditableEntity
    {
        public string? Name { get; set; }
        public bool? IsConcoction { get; set; }
        public long? PhycisianId { get; set; }
        public long? UoMId { get; set; }
        public long? FormDrugId { get; set; }

        [SetToNull]
        public virtual User? Phycisian { get; set; }
        [SetToNull]
        public virtual Uom? UoM { get; set; }
        [SetToNull]
        public virtual DrugForm? FormDrug { get; set; }
    }
}
