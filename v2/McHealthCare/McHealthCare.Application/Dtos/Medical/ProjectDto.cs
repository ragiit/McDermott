using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ProjectDto: IMapFrom<Project>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }
    }
    public class CreateUpdateProjectDto : IMapFrom<Project>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }
    }
}
