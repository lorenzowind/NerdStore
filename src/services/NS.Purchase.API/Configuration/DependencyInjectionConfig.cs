using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NS.Purchase.API.Data;
using NS.Purchase.API.Data.Repository;
using NS.Purchase.API.Facade;
using NS.Purchase.API.Models;
using NS.Purchase.API.Services;
using NS.WebAPI.Core.User;

namespace NS.Purchase.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Application
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPurchaseFacade, CreditCardPurchaseFacade>();

            // Data
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<PurchaseContext>();
        }
    }
}
