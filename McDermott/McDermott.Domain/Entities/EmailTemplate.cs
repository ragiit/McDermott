using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class EmailTemplate:BaseAuditableEntity
    {
        [StringLength(200)]
        public string? Subject { get; set; }
        [StringLength(200)] 
        public string? from { get; set; }
        public int? ById { get; set; }
        [StringLength(200)]
        public string? To { get; set; }
        public int? ToPartnerId { get; set; }
        [StringLength(200)]
        public string? Cc { get; set; }
        public string? ReplayTo { get; set; }
        public DateTime? Schendule {  get; set; }
        public string? Message {  get; set; }

        public virtual User? By {  get; set; }
        public virtual List<User>? ToPartner { get; set; }

    }
}
