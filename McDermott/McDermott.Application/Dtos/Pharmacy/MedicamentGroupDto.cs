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
        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; } = string.Empty;
        public bool IsConcoction { get; set; } = false;
        public long? PhycisianId { get; set; }
        public long? UoMId { get; set; }
        public long? FormDrugId { get; set; }

        public virtual UserDto? Phycisian { get; set; }
        public virtual Uom? UoM { get; set; }
        public virtual DrugFormDto? FromDrug { get; set; }
        
       
    }
}
