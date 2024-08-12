using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McHealthCare.Domain.Entities;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class EmployeeDto : IMapFrom<Employee>
    {
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public Guid? OccupationalId { get; set; }
        public string? SupervisorId { get; set; }
        public Guid? JobPositionId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? NoBpjsKs { get; set; }
        public string? NoBpjsTk { get; set; }
        public string? NIP { get; set; }
        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }
        public string? EmployeeType { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
