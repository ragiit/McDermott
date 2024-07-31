using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Common.Interfaces;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Application.Dtos.Medical
{
    public  class ServicedDto : IMapFrom<Service>{
        [StringLength(200)]
        public string Name {get;set;} = string.Empty;
        [StringLength(5)]
        public string code {get;set;} = string.Empty;
        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual ServicedDto? Serviced { get; set;}
    }

    public class CreateUpdateServiceDto : IMapFrom<Service>
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        [StringLength(5)]
        public string code { get; set; } = string.Empty;
        public string Quota { get; set; } = string.Empty;
        public bool IsPatient { get; set; }
        public bool IsKiosk { get; set; }
        public bool IsMcu { get; set; } = false;
        public Guid? ServicedId { get; set; }

        [SetToNull]
        public virtual ServicedDto? Serviced { get; set; }
    }
}