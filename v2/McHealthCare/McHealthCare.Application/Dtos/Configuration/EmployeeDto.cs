namespace McHealthCare.Application.Dtos.Configuration
{
    public class EmployeeDto : IMapFrom<McHealthCare.Domain.Entities.Configuration.Employee>
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
        //public virtual ApplicationUserDto? ApplicationUser { get; set; }
        //public virtual EmployeeDto? Supervisor { get; set; }
        //public virtual OccupationalDto? Occupational { get; set; }
    }

    public class CreateUpdateEmployeeDto
    {
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