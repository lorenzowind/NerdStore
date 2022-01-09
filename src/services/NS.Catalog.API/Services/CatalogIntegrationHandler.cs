using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NS.Catalog.API.Models;
using NS.Core.DomainObjects;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Catalog.API.Services
{
    public class CatalogIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CatalogIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
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
            _bus.SubscribeAsync<AuthorizedOrderIntegrationEvent>("AuthorizedOrder", 
                async request => await SetStorage(request));
        }

        private async Task SetStorage(AuthorizedOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productsWithStorage = new List<Product>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var productsIds = string.Join(",", message.Itens.Select(c => c.Key));
                var products = await productRepository.GetProductsById(productsIds);

                if (products.Count != message.Itens.Count)
                {
                    CancelNoStorageOrder(message);
                    return;
                }

                foreach (var product in products)
                {
                    var productQuantity = message.Itens.FirstOrDefault(p => p.Key == product.Id).Value;

                    if (product.IsAvailable(productQuantity))
                    {
                        product.DecreaseStorage(productQuantity);
                        productsWithStorage.Add(product);
                    }
                }

                if (productsWithStorage.Count != message.Itens.Count)
                {
                    CancelNoStorageOrder(message);
                    return;
                }

                foreach (var product in productsWithStorage)
                {
                    productRepository.Update(product);
                }

                if (!await productRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"An error occurred to update storage of order {message.OrderId}");
                }

                var setOrder = new SetStorageOrderIntegrationEvent(message.CustomerId, message.OrderId);
                await _bus.PublishAsync(setOrder);
            }
        }

        public async void CancelNoStorageOrder(AuthorizedOrderIntegrationEvent message)
        {
            var cancelledOrder = new CancelledOrderIntegrationEvent(message.CustomerId, message.OrderId);
            await _bus.PublishAsync(cancelledOrder);
        }
    }
}
