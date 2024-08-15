using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class Signa : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<Medicament>? Medicaments { get; set; }
    }
}
