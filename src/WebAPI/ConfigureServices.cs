using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Infrastructure.Persistence;
using WebApi.Filters;

namespace WebApi;
public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddEndpointsApiExplorer();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddHttpContextAccessor();

        services.AddFluentValidationClientsideAdapters();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);



        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "MicroLogix PLC - API",
                Description = "API for Plcs and PlcTags on the PLC",
                Contact = new OpenApiContact
                {
                    Name = "Jared West"
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });



        return services;
    }
}
