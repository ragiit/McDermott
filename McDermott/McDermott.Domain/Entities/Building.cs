using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public partial class Building : BaseAuditableEntity
    {
        public int? HealthCenterId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Code { get; set; } = string.Empty;

        public virtual HealthCenter? HealthCenter { get; set; }

        public virtual List<BuildingLocation>? BuildingLocations { get; set; }

    }
}