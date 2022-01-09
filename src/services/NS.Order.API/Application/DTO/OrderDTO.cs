using System;
using System.Collections.Generic;

namespace NS.Order.API.Application.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public Guid CustomerId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool AppliedVoucher { get; set; }

        public List<OrderItemDTO> OrderItens { get; set; }
        public AddressDTO Address { get; set; }

        public static OrderDTO ToOrderDTO(Domain.Orders.Order order)
        {
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                Date = order.RegistrationDate,
                TotalPrice = order.TotalPrice,
                Discount = order.Discount,
                AppliedVoucher = order.AppliedVoucher,
                OrderItens = new List<OrderItemDTO>(),
                Address = new AddressDTO()
            };

            foreach (var item in order.OrderItens)
            {
                orderDTO.OrderItens.Add(new OrderItemDTO
                {
                    Name = item.ProductName,
                    Image = item.ProductImage,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    Price = item.UnitaryPrice,
                    OrderId = item.OrderId
                });
            }

            orderDTO.Address = new AddressDTO
            {
                PublicArea = order.Address.PublicArea,
                Number = order.Address.Number,
                Complement = order.Address.Complement,
                District = order.Address.District,
                ZipCode = order.Address.ZipCode,
                City = order.Address.City,
                State = order.Address.State,
            };

            return orderDTO;
        }
    }
}
