using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Pharmacies
{
    public class MedicamentGroup :BaseAuditableEntity
    {
        public string? Name { get; set; }
        public bool? IsConcoction { get; set; }
        public string? PhycisianId { get; set; }
        public Guid? UoMId { get; set; }
        public Guid? FormDrugId { get; set; }

        [SetToNull]
        public virtual Doctor? Phycisian { get; set; }
        [SetToNull]
        public virtual Uom? UoM { get; set; }
        [SetToNull]
        public virtual DrugForm? FormDrug { get; set; }
    }
}
