using McHealthCare.Application.Interfaces;
using McHealthCare.Context;
using McHealthCare.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Persistence.Extentions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMappings();
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        //private static void AddMappings(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //}

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(connectionString,
            //       builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            //services.AddDbContextFactory<ApplicationDbContext>(options =>
            //   options.UseSqlServer(connectionString), ServiceLifetime.Transient);

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //    options.UseSqlServer(connectionString);
            //}, ServiceLifetime.Transient);
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                /*.AddTransient<IStadiumRepository, StadiumRepository>()*/;
        }
    }
}