using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NS.Cart.API.Data;
using NS.WebAPI.Core.User;

namespace NS.Cart.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<CartContext>();
        }
    }
}
