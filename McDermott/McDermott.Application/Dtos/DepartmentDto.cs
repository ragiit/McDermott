using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}