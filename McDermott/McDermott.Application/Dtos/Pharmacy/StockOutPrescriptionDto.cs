using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class StockOutPrescriptionDto:IMapFrom<StockOutPrescription>
    {
        public long Id { get; set; }
        public long? PrescriptionId { get; set; }
        public long? StockId { get; set; }
        [Required(ErrorMessage ="Input Stock Not Null!!")]
        public long? CutStock {  get; set; }
        public string? Batch { get; set; }
        public long? CurrentStock { get; set; }
        public DateTime? Expired { get; set; }

        public virtual PrescriptionDto? Prescription { get; set; }
        public virtual StockProductDto? Stock {  get; set; }

    }
}
