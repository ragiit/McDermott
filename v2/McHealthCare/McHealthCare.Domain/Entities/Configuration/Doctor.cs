using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities
{
    public class Doctor
    {
        public Guid? SpecialityId { get; set; } 
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }  
        public bool DoctorType { get; set; }
        public bool IsPhysicion { get; set; } = false;
        public bool IsNurse { get; set; } = false;
        public string? PhysicanCode { get; set; }
        public string? SipNo { get; set; }
        public string? SipFile { get; set; }
        public string? SipFileBase64 { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public string? StrFileBase64 { get; set; }
        public DateTime? StrExp { get; set; } 

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}