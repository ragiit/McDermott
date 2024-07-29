using McHealthCare.Domain.Common;
using McHealthCare.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities
{
    public class Province : BaseAuditableEntity
    {
        public Guid CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public virtual Country? Country { get; set; }
         
    }
}