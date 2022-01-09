using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NS.Core.DomainObjects;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using NS.Purchase.API.Models;

namespace NS.Purchase.API.Services
{
    public class PurchaseIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PurchaseIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<InitializedOrderIntegrationEvent, ResponseMessage>(async request =>
                await AuthorizePurchase(request));
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<CancelledOrderIntegrationEvent>("CancelledOrder", async request =>
            await CancelPurchase(request));

            _bus.SubscribeAsync<SetStorageOrderIntegrationEvent>("SetStorageOrder", async request =>
            await InterceptPurchase(request));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> AuthorizePurchase(InitializedOrderIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var purchaseService = scope.ServiceProvider.GetRequiredService<IPurchaseService>();
            var purchase = new Models.Purchase
            {
                OrderId = message.OrderId,
                PaymentType = (PaymentType)message.PaymentType,
                Price = message.Price,
                CreditCard = new CreditCard(message.CardName, message.CardNumber, 
                    message.ExpirationMonthYear, message.CVV)
            };

            var response = await purchaseService.AuthorizePurchase(purchase);

            return response;
        }

        private async Task CancelPurchase(CancelledOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var purchaseService = scope.ServiceProvider.GetRequiredService<IPurchaseService>();

                var response = await purchaseService.CancelPurchase(message.OrderId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"An error occurred to cancel purchase of order {message.OrderId}");
            }
        }

        private async Task InterceptPurchase(SetStorageOrderIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var purchaseService = scope.ServiceProvider.GetRequiredService<IPurchaseService>();

                var response = await purchaseService.InterceptPurchase(message.OrderId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"An error occurred to intercept purchase of order {message.OrderId}");

                await _bus.PublishAsync(new PaidOrderIntegrationEvent(message.CustomerId, message.OrderId));
            }
        }
    }
}