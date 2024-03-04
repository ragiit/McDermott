using McDermott.Application.Features.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace McDermott.Application.Extentions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediator();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddMemoryCache(); // Menambahkan layanan memory cache
            services.AddDistributedMemoryCache(); // Menambahkan layanan distributed memory cache (opsional, digunakan jika Anda memerlukan cache di beberapa instance server) 

            //var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //services.AddScoped<CustomAuthenticationStateProvider>(provider => new CustomAuthenticationStateProvider(authenticationState));
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}