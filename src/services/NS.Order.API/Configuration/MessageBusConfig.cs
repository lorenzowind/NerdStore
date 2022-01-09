using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NS.Core.Utils;
using NS.MessageBus;
using NS.Order.API.Services;

namespace NS.Order.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<OrderOrchestratorIntegrationHandler>();
        }
    }
}
