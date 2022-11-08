using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Infrastructure.Files;
using NoRslinx.Infrastructure.Persistence;
using NoRslinx.Infrastructure.Persistence.Interceptors;
using NoRslinx.Infrastructure.Services;

namespace NoRslinx.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("NoRslinxDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();


        services.AddTransient<IDateTime, DateTimeService>();

        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        //services.AddTransient<IRsLogixDbImporter, RslogixDbImporter>();




        return services;
    }
}
