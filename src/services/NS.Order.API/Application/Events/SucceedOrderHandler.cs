using MediatR;
using NS.Core.Messages.Integration;
using NS.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Order.API.Application.Events
{
    public class SucceedOrderHandler : INotificationHandler<SucceedOrderEvent>
    {
        private readonly IMessageBus _bus;

        public SucceedOrderHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(SucceedOrderEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new SucceedOrderIntegrationEvent(message.CustomerId));
        }
    }
}
