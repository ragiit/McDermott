using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class FormDrugDto :IMapFrom<FormDrug>
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
