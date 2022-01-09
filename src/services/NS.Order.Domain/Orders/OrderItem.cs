using NS.Core.DomainObjects;
using System;

namespace NS.Order.Domain.Orders
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitaryPrice { get; private set; }
        public string ProductImage { get; set; }

        public Order Order { get; private set; }

        public OrderItem(Guid productId, string productName, int quantity, 
            decimal unitaryPrice, string productImage = null)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitaryPrice = unitaryPrice;
            ProductImage = productImage;
        }

        protected OrderItem() { }

        internal decimal CalculatePrice()
        {
            return Quantity * UnitaryPrice;
        }
    }
}
