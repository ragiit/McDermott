using McDermott.Application.Interfaces.Repositories;
using McDermott.Persistence.Context;
using McDermott.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace McDermott.Persistence.Extensions
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
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine(connectionString);

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

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Mengaktifkan retry pada kegagalan sementara
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,  // Jumlah maksimal percobaan ulang
                        maxRetryDelay: TimeSpan.FromSeconds(10),  // Jeda waktu antara percobaan ulang
                        errorNumbersToAdd: null  // Nomor error SQL yang akan ditambahkan ke daftar retry
                    );

                    sqlOptions.CommandTimeout(60);  // Timeout untuk eksekusi perintah
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    sqlOptions.MaxBatchSize(100);  // Batas maksimal batch dalam satu transaksi
                });

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }, ServiceLifetime.Transient);
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