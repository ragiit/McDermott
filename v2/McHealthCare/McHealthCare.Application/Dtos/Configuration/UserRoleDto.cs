namespace McHealthCare.Application.Dtos.Configuration
{
    public class UserRoleDto
    {
        public bool IsUser { get; set; }
        public bool IsPractitioner { get; set; }
        public bool IsPatient { get; set; }
        public bool IsMCU { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsPharmacy { get; set; }
        public bool IsHR { get; set; }
        public bool IsAdmin { get; set; }
    }
}