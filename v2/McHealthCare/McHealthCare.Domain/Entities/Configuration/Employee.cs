using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities
{
    public class Employee
    {
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        public string? NIP { get; set; }
        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}