using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MedicamentGroupDetail : BaseAuditableEntity
    {
        public long? MedicamentGroupId { get; set; }
        public long? MedicaneId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public long? UoMId { get; set; }
        public float? MedicaneUnitDosage { get; set; }
        public float? QtyByDay { get; set; }
        public float? Days { get; set; }
        public float? TotalQty { get; set; }
        public string? MedicaneName { get; set; }
        public string? UnitOfDosage { get; set; }
        public string? UnitOfDosageCategory { get; set; }
        public string? Comment { get; set; }

        [SetToNull]
        public virtual List<ActiveComponent>? ActiveComponent { get; set; }
        [SetToNull]
        public virtual MedicamentGroup? MedicamentGroup { get; set; }
        [SetToNull]
        public virtual Uom? UoM { get; set; }
    }
}
