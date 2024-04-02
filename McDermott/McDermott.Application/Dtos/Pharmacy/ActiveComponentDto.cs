namespace McDermott.Application.Dtos.Pharmacy
{
    public class ActiveComponentDto : IMapFrom<ActiveComponent>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? AmountOfComponent { get; set; }
    }
}
