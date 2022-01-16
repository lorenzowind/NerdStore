using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NS.Buyer.AGW.Services.gRPC;
using NS.Cart.API.Services.gRPC;
using NS.WebAPI.Core.Extensions;

namespace NS.Buyer.AGW.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICartGrpcService, CartGrpcService>();

            services.AddGrpcClient<BuyerCart.BuyerCartClient>(options =>
            {
                options.Address = new Uri(configuration["CartUrl"]);
            })
                .AddInterceptor<GrpcServiceInterceptor>()
                .AllowSelfSignedCertificate();
        }
    }
}