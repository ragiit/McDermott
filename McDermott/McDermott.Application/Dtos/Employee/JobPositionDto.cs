using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Employee
{
    public class JobPositionDto : IMapFrom<JobPosition>
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual DepartmentDto? Department { get; set; }
    }
}