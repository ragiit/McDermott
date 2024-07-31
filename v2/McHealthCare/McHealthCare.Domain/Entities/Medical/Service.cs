using McHealthCare.Domain.Common;
using McHealthCare.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities.Medical
{
    public partial class Service : BaseAuditableEntity{
        [StringLength(200)]
        public string? Name {get;set;}
        [StringLength(5)]
        public string? code {get;set;}
        public string? Quota { get; set; }
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual Service? Serviced { get; set;}
    }
}