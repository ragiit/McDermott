using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class MedicamentGroupDetail
    {
        public long? MegacamentGroupId { get; set;}
        public int? MedicaneId { get; set; }
        public int? ActiveComponent { get; set; }
        public int? RegimentOfUseId { get; set; }
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
