using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NS.Core.DomainObjects;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using NS.Order.Domain.Orders;

namespace NS.Order.API.Services
{
    public class OrderIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public OrderIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("CancelledOrder",
                async request => await CancelOrder(request));

            _bus.SubscribeAsync<PaidOrderIntegrationEvent>("PaidOrder",
               async request => await ConcludeOrder(request));
        }

        private async Task CancelOrder(CancelledOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var order = await orderRepository.GetById(message.OrderId);
                order.CancelOrder();

                orderRepository.Update(order);

                if (!await orderRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"An error occurred to cancel order {message.OrderId}");
                }
            }
        }

        private async Task ConcludeOrder(PaidOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var order = await orderRepository.GetById(message.OrderId);
                order.ConcludeOrder();

                orderRepository.Update(order);

                if (!await orderRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"An error occurred to conclude order {message.OrderId}");
                }
            }
        }
    }
}