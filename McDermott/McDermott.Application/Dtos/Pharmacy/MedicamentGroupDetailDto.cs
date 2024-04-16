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
        public long? MedicamentGroupId { get; set; }
        public long? MedicamentId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public long? SignaId { get; set; }
        public long? RegimentOfUseId { get; set; }
        public bool? AllowSubtitation { get; set; } = false;
        public string? MedicaneUnitDosage { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? MedicaneDosage { get; set; }
        
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? Dosage { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? QtyByDay { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? Days { get; set; }
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? TotalQty { get; set; }
        public string? MedicaneName { get; set; }
        public string? Comment { get; set; }
    }
}
