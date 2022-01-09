using NS.Core.Messages;
using System;

namespace NS.Order.API.Application.Events
{
    public class SucceedOrderEvent : Event
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public SucceedOrderEvent(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}
