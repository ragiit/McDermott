namespace McDermott.Application.Dtos.Pharmacy
{
    public class ActiveComponentDto : IMapFrom<ActiveComponent>
    {
        public long Id { get; set; }
        public long? UomId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int? AmountOfComponent { get; set; }

        public UomDto? Uom { get; set; }
    }
}