using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace McHealthCare.Application.Extentions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediator();
            services.AddMemoryCache();  // Menambahkan MemoryCache// Register DataService
            //services.AddScoped<DataService>();
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}