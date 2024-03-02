namespace McDermott.Application.Features.Services
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
    }
}