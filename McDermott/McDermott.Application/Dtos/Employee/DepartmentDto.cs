using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Employee
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public int Id { get; set; }  

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }

        [StringLength(200)]
        public string? ParentName { get; set; }

        public int? CompanyId { get; set; }
        public string? DepartmentCategory { get; set; }
        public string? Manager { get; set; }
        public virtual CompanyDto? Company { get; set; }  
    }
}