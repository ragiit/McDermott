using McHealthCare.Application.Features.CommandsQueries.Employee;
using McHealthCare.Domain.Entities;
using McHealthCare.Domain.Entities.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Employee
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? ManagerId { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DepartmentCategory { get; set; }
        public virtual ApplicationUserDto? Manager { get; set; }
        public virtual DepartmentDto? ParentDepartment { get; set; }
        public virtual CompanyDto? Company { get; set; }
    }


    public class CreateUpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? ManagerId { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DepartmentCategory { get; set; } 
    }
}
