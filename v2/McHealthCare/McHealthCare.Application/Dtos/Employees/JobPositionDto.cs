using McHealthCare.Application.Dtos.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Employees
{
    public class JobPositionDto : IMapFrom<JobPosition>
    {
        public Guid Id { get; set; }
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public DepartmentDto? Department { get; set; }
    }

    public class CreateUpdateJobPositionDto  
    {
        public Guid Id { get; set; }    
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; 
    }
}
