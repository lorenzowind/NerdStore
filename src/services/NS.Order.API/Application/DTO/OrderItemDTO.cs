using NS.Order.Domain.Orders;
using System;

namespace NS.Order.API.Application.DTO
{
    public class OrderItemDTO
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public static OrderItem ToOrderItem(OrderItemDTO orderItemDTO)
        {
            return new OrderItem(orderItemDTO.ProductId, orderItemDTO.Name, orderItemDTO.Quantity,
                orderItemDTO.Price, orderItemDTO.Image);
        }
    }
}
