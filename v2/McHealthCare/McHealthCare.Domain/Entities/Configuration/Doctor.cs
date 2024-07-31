using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities
{
    public class Doctor
    {
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }  // Primary key and foreign key 
        public string? SipFile { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public DateTime? StrExp { get; set; }


        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
