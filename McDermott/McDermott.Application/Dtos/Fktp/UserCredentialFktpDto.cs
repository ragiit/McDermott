namespace McDermott.Application.Dtos.Fktp
{
    public record UserCredentialFktpDto : IMapFrom<UserCredentialFktp>
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}