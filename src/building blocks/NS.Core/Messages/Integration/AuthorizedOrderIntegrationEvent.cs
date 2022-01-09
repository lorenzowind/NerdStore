using System;
using System.Collections.Generic;

namespace NS.Core.Messages.Integration
{
    public class AuthorizedOrderIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public IDictionary<Guid, int> Itens { get; private set; }

        public AuthorizedOrderIntegrationEvent(Guid customerId, Guid orderId, IDictionary<Guid, int> itens)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Itens = itens;
        }
    }
}