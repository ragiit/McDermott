using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Products
{
    public class DrugForm :BaseAuditableEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
