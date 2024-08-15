namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class ActiveComponentDto : IMapFrom<ActiveComponent>
    {
        public Guid Id { get; set; }
        public Guid? UomId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? AmountOfComponent { get; set; }

        public UomDto? Uom { get; set; }
    }
}