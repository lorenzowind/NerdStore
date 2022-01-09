using NS.Core.DomainObjects;
using NS.Order.Domain.Vouchers;
using System;
using System.Collections.Generic;

namespace NS.Order.Domain.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool AppliedVoucher { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItens;
        public IReadOnlyCollection<OrderItem> OrderItens => _orderItens;

        public Address Address { get; private set; }

        public Voucher Voucher { get; private set; }

        public Order(Guid customerId, decimal totalPrice, List<OrderItem> orderItems,
            bool appliedVoucher = false, decimal discount = 0, Guid? voucherId = null)
        {
            CustomerId = customerId;
            TotalPrice = totalPrice;
            _orderItens = orderItems;

            Discount = discount;
            AppliedVoucher = appliedVoucher;
            VoucherId = voucherId;
        }

        protected Order() { }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorized;
        }
        public void CancelOrder()
        {
            OrderStatus = OrderStatus.Cancelled;
        }
        public void ConcludeOrder()
        {
            OrderStatus = OrderStatus.Paid;
        }

        public void ApplyVoucher(Voucher voucher)
        {
            AppliedVoucher = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        public void CalculateCartPriceWithDiscount()
        {
            if (!AppliedVoucher) return;

            decimal discount = 0;
            var price = TotalPrice;

            if (Voucher.DiscountType == DiscountVoucherType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (price * Voucher.Percentage.Value) / 100;
                    price -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;

                    price -= discount;
                }
            }

            TotalPrice = price < 0 ? 0 : price;
            Discount = discount;
        }

    }
}
