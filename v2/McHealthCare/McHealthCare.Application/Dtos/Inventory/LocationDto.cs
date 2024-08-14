using McHealthCare.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Inventory
{
    public class LocationDto : IMapFrom<Location>
    {
        public Guid Id { get; set; }
        public Guid? ParentLocationId { get; set; }
        public Guid? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;

        public virtual LocationDto? ParentLocation { get; set; }
        public virtual CompanyDto? Company { get; set; }
    }

    public class CreateUpdateLocationDto
    {
        public Guid Id { get; set; }
        public Guid? ParentLocationId { get; set; }
        public Guid? CompanyId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Type { get; set; } = string.Empty;
    }
}
