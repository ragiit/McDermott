using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MedicamentGroupDetail : BaseAuditableEntity
    {
        public long MedicamentGroupId { get; set; }
        public long? MedicamentId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public long? SignaId { get; set; }
        public long? RegimentOfUseId { get; set; }
        public bool? AllowSubtitation { get; set; }
        public string? MedicaneUnitDosage { get; set; }       
        public string? MedicaneDosage { get; set; }        
        public string? Dosage { get; set; }        
        public string? QtyByDay { get; set; }        
        public string? Days { get; set; }        
        public string? TotalQty { get; set; }
        public string? MedicaneName { get; set; }
        public string? Comment { get; set; }

        [SetToNull]
        public virtual List<ActiveComponent>? ActiveComponent { get; set; }
        [SetToNull]
        public virtual MedicamentGroup? MedicamentGroup { get; set; }
        [SetToNull]
        public virtual Uom? UoM { get; set; }
        [SetToNull]
        public virtual Medicament? Medicament { get; set; }
    }
}
