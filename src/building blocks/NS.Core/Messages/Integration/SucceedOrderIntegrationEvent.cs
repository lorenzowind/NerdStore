using System;

namespace NS.Core.Messages.Integration
{
    public class SucceedOrderIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }

        public SucceedOrderIntegrationEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
