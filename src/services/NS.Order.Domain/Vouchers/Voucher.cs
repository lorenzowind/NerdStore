using NS.Core.DomainObjects;
using NS.Order.Domain.Vouchers.Specs;
using System;

namespace NS.Order.Domain.Vouchers
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Quantity { get; private set; }
        public DiscountVoucherType DiscountType { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? UsageDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public bool IsValid()
        {
            return new VoucherActiveSpecification()
                .And(new VoucherDateSpecification())
                .And(new VoucherQuantitySpecification())
                .IsSatisfiedBy(this);
        }

        public void SetToUsed()
        {
            Active = false;
            Used = true;
            Quantity = 0;
        }

        public void DecreaseQuantity()
        {
            Quantity--;
            if (Quantity >= 1) return;

            SetToUsed();
        }
    }
}
