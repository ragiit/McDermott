using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Domain.Entities
{
    public class HealthCenter : BaseAuditableEntity
    {
        public int? CityId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? WebsiteLink { get; set; }

        public virtual City? City { get; set; }
        public virtual Province? Province { get; set; }
        public virtual Country? Country { get; set; }

        public virtual List<Building>? Buildings { get; set; }
    }
}