using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ProcedureDto : IMapFrom<Procedure>
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Classification { get; set; }
    }

    public class CreateUpdateProcedureDto : IMapFrom<Procedure>
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Classification { get; set; }
    }
}