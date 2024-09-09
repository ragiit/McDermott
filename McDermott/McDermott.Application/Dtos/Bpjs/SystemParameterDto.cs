namespace McDermott.Application.Dtos.Bpjs
{
    public class SystemParameterDto : IMapFrom<SystemParameter>
    {
        public long Id { get; set; }

        //[Required]
        //public string Key { get; set; } = string.Empty;

        //public string? Value { get; set; }

        [Required]
        public string? PCareBaseURL { get; set; }

        [Required]
        public string? AntreanFKTPBaseURL { get; set; }

        [Required]
        public string? ConsId { get; set; }

        [Required]
        public string? KdAplikasi { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? PCareCodeProvider { get; set; }

        [Required]
        public string? SecretKey { get; set; }

        [Required]
        public string? UserKey { get; set; }

        [Required]
        public string? Username { get; set; }
    }
}