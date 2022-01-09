using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using NS.Order.API.Application.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Order.API.Services
{
    public class OrderOrchestratorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderOrchestratorIntegrationHandler> _logger;
        private Timer _timer;

        public OrderOrchestratorIntegrationHandler(IServiceProvider serviceProvider, 
            ILogger<OrderOrchestratorIntegrationHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order service started");

            _timer = new Timer(ProcessOrders, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private async void ProcessOrders(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();
                var order = await orderQueries.GetAuthorizedOrders();

                if (order == null) return;

                var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
                var authorizedOrder = new AuthorizedOrderIntegrationEvent(order.CustomerId, order.Id,
                    order.OrderItens.ToDictionary(i => i.ProductId, i => i.Quantity));

                await bus.PublishAsync(authorizedOrder);

                _logger.LogInformation($"Order ID: {order.Id} was sent for storage alteration");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order service stopped");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
