using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class MedicamentGroupDto :IMapFrom<MedicamentGroup>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsConcoction { get; set; } = false;
        public long? PhycisianId { get; set; }
        public long? RegimentOfUseId { get; set; }
        public long? FormDrugId { get; set; }
        

        
    }
}
