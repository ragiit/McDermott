namespace McDermott.Application.Dtos.Medical
{
    public class LabUomDto : IMapFrom<LabUom>
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}
