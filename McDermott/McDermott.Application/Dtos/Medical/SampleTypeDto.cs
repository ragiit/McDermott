namespace McDermott.Application.Dtos.Medical
{
    public class SampleTypeDto : IMapFrom<SampleType>
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}