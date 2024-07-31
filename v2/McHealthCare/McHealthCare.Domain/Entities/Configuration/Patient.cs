using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Entities.Configuration
{
    public class Patient
    {
        [Key, ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }  // Primary key and foreign key

        public string? NoRm { get; set; } = "-";

        public virtual ApplicationUser? ApplicationUser { get; set; }  // Navigation property
    }
}
