using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Employees
{
    public class JobPosition : BaseAuditableEntity
    {
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
         
        public Department? Department { get; set; }
         
    }
}
