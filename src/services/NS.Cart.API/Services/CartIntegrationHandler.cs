using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NS.Cart.API.Data;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Cart.Services
{
    public class CartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<SucceedOrderIntegrationEvent>("SucceedOrder", async request => await RemoveCart(request));
        }

        private async Task RemoveCart(SucceedOrderIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartContext>();

            var cart = await context.CustomerCarts.FirstOrDefaultAsync(c => c.CustomerId == message.CustomerId);

            if (cart == null)
            {
                context.CustomerCarts.Remove(cart);
                await context.SaveChangesAsync();
            }
        }
    }
}
