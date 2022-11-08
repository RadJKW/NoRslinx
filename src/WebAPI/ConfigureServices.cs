using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Infrastructure.Persistence;
using WebApi.Filters;

namespace WebApi;
public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        services.AddEndpointsApiExplorer();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddHttpContextAccessor();

        services.AddFluentValidationAutoValidation();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddSwaggerGen();

        return services;
    }
}
