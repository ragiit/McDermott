using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class InsuranceDto : IMapFrom<Insurance>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; } 
        [StringLength(5)]
        public string? Code { get; set; }
        public string? Type { get; set; }
        public bool? IsBPJSKesehatan { get; set; }
        public bool? IsBPJSTK { get; set; }
        public int? AdminFee { get; set; }
        public int? Percentage { get; set; }
        public int? AdminFeeMax { get; set; }
    }

    public class CreateUpdateInsuranceDto : IMapFrom<Insurance>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(5)]
        public string? Code { get; set; }
        public string? Type { get; set; }
        public bool? IsBPJSKesehatan { get; set; }
        public bool? IsBPJSTK { get; set; }
        public int? AdminFee { get; set; }
        public int? Percentage { get; set; }
        public int? AdminFeeMax { get; set; }
    }
}