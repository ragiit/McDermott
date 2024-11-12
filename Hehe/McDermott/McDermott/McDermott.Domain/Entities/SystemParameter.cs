namespace McDermott.Domain.Entities
{
    public class SystemParameter : BaseAuditableEntity
    {
        //public string Key { get; set; } = string.Empty;
        //public string? Value { get; set; }
        public string? PCareBaseURL { get; set; }

        public string? AntreanFKTPBaseURL { get; set; }
        public string? ConsId { get; set; }
        public string? KdAplikasi { get; set; }
        public string? Password { get; set; }
        public string? PCareCodeProvider { get; set; }
        public string? SecretKey { get; set; }
        public string? UserKey { get; set; }
        public string? Username { get; set; }
    }
}