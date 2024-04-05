using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class Product :BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<Medicament>? Medicaments { get; set; }
        public List<GeneralInformation>? GeneralInformation { get; set; }
    }
}
