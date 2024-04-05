using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class MedicamentDto:IMapFrom<Medicament>
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? SignaId { get; set; }
        public long? RouteId { get; set; }
        public long? UomId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public bool? PregnancyWarning { get; set; }
        public bool? Cronies { get; set; }
        public string? MontlyMax { get; set; }
        public string? Form { get; set; }
        public string? Dosage { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }
        [SetToNull]
        public virtual SignaDto? Signa { get; set; }
        [SetToNull]
        public virtual DrugRouteDto? Route { get; set; }
        [SetToNull]
        public virtual UomDto? Uom { get; set; }
        [SetToNull]
        public virtual List<ActiveComponentDto>? ActiveComponent { get; set; }
        
    }
}
