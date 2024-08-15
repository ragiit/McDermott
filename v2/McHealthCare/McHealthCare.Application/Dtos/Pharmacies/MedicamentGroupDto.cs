namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class MedicamentGroupDto : IMapFrom<MedicamentGroup>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name Must Be Filled In!")]
        public string? Name { get; set; } = string.Empty;

        public bool IsConcoction { get; set; } = false;
        public string? PhycisianId { get; set; }
        public Guid? UoMId { get; set; }
        public Guid? FormDrugId { get; set; }

        public virtual DoctorDto? Phycisian { get; set; }
        public virtual Uom? UoM { get; set; }
        public virtual DrugFormDto? FromDrug { get; set; }
    }
}