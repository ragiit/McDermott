namespace McHealthCare.Application.Dtos.Bpjs
{
    public class BpjsClassificationDto : IMapFrom<BpjsClassification>
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }
    }
}