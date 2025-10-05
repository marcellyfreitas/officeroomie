using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApi.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "WebApi",
                Description = "WebApi - Web APIs",
                Version = "v1"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }
}