using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class MedicamentGroupDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsConcoction { get; set; } = false;
        public long? PhycisianId { get; set; }
        public long? MedicaneId { get; set; }
        public long? ActiveComponent { get; set; }
        public long? RegimentOfUseId { get; set; }
        public float? MedicaneUnitDosage { get; set; }
        public float? QtyByDay { get; set; }
        public float? Days { get; set; }
        public float? TotalQty { get; set; }
        public string? MedicaneName { get; set; }
        public string? UnitOfDosage { get; set; }
        public string? Form { get; set; }
        public string? UnitOfDosageCategory { get; set; }
        public string? Comment { get; set; }

        
    }
}
