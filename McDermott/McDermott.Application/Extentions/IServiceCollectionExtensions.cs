using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace McDermott.Application.Extentions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediator();

            //var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //services.AddScoped<CustomAuthenticationStateProvider>(provider => new CustomAuthenticationStateProvider(authenticationState));
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}