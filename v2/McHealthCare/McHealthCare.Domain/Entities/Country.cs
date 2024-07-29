using McHealthCare.Domain.Common;
using McHealthCare.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities
{
    public class Country : BaseAuditableEntity, INotifiable
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public virtual List<Province>? Provinces { get; set; }

        [NotMapped]
        public string Type => "Country";
        [NotMapped]
        public object Data => this;
    }
}