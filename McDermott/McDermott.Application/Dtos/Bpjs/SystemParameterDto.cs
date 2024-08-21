namespace McDermott.Application.Dtos.Bpjs
{
    public class SystemParameterDto : IMapFrom<SystemParameter>
    {
        public long Id { get; set; }

        [Required]
        public string Key { get; set; } = string.Empty;

        public string? Value { get; set; }
    }
}