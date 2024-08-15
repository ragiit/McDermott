using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Employee
{
    public class Department : BaseAuditableEntity
    {
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? ManagerId { get; set; } 
        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty; 
        public string? DepartmentCategory { get; set; } 
        public virtual ApplicationUser? Manager { get; set; }  
        public virtual Department? ParentDepartment { get; set; } 
        public virtual Company? Company { get; set; }
    }
}
