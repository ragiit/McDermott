using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ServiceDto : IMapFrom<Service>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public string? ServiceCounter { get; set; }
        public string? Flag { get; set; }
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual ServiceDto? Serviced { get; set; }
    }

    public class CreateUpdateServiceDto : IMapFrom<Service>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public string? ServiceCounter { get; set; }
        public string? Flag { get; set; }
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual ServiceDto? Serviced { get; set; }
    }
}