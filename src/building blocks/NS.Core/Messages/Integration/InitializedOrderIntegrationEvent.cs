using System;

namespace NS.Core.Messages.Integration
{
    public class InitializedOrderIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public int PaymentType { get; set; }
        public decimal Price { get; set; }

        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonthYear { get; set; }
        public string CVV { get; set; }
    }
}
