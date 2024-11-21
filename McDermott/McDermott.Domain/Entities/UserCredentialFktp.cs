namespace McDermott.Domain.Entities
{
    public class UserCredentialFktp : BaseAuditableEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}