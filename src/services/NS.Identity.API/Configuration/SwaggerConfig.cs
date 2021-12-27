using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace NS.Identity.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var licenseUri = "https://opensource.org/licenses/MIT";

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NS.Identity.API",
                    Description = "This API is part of the NerdStore Web Application",
                    Contact = new OpenApiContact() { Name = "Lorenzo Windmoller", Email = "lorenzomart01@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri(licenseUri) },
                    Version = "v1"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NS.Identity.API v1"));

            return app;
        }
    }
}
