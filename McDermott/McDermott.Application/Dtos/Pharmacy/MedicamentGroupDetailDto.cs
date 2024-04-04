using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class MedicamentGroupDetailDto :IMapFrom<MedicamentGroupDetail>
    {
        public long Id { get; set; }
        public long MedicamentGroupId { get; set; }
        public long? MedicamentId { get; set; }
        public long? ActiveComponent { get; set; }
        public float? MedicaneUnitDosage { get; set; }
        public float? QtyByDay { get; set; }
        public float? Days { get; set; }
        public float? TotalQty { get; set; }
        public string? MedicaneName { get; set; }
        public string? UnitOfDosage { get; set; }
        public string? UnitOfDosageCategory { get; set; }
        public string? Comment { get; set; }
    }
}
