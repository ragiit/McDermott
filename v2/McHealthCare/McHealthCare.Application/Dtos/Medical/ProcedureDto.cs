using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ProcedureDto : IMapFrom<Procedure>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Classification { get; set; }
    }

    public class CreateUpdateProcedureDto : IMapFrom<Procedure>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(100)]
        public string? Classification { get; set; }
    }
}