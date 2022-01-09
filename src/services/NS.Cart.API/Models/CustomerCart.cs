using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NS.Cart.API.Models
{
    public class CustomerCart
    {
        internal const int MAX_QUANTITY_ITENS = 5;

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> Itens { get; set; } = new List<CartItem>();

        public ValidationResult ValidationResult { get; set; }

        public bool AppliedVoucher { get; set; }
        public decimal Discount { get; set; }

        public Voucher Voucher { get; set; }

        public CustomerCart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CustomerCart()
        {
        }

        public void ApplyVoucher(Voucher voucher)
        {
            Voucher = voucher;
            AppliedVoucher = true;
            CalculateCartPrice();
        }

        private void CalculateCartPriceWithDiscount()
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

        internal void CalculateCartPrice()
        {
            TotalPrice = Itens.Sum(i => i.CalculatePrice());
            CalculateCartPriceWithDiscount();
        }

        internal bool IsExistingCartItem(CartItem item)
        {
            return Itens.Any(i => i.ProductId == item.ProductId);
        }

        internal CartItem GetCartItemByProductId(Guid productId)
        {
            return Itens.FirstOrDefault(i => i.ProductId == productId);
        }

        internal void AddItem(CartItem item)
        {
            item.LinkCart(Id);

            if (IsExistingCartItem(item))
            {
                var existingItem = GetCartItemByProductId(item.ProductId);
                existingItem.IncrementQuantity(item.Quantity);

                item = existingItem;
                Itens.Remove(existingItem);
            }

            Itens.Add(item);
            CalculateCartPrice();
        }

        internal void UpdateItem(CartItem item)
        {
            item.LinkCart(Id);

            var existingItem = GetCartItemByProductId(item.ProductId);

            Itens.Remove(existingItem);
            Itens.Add(item);

            CalculateCartPrice();
        }

        internal void UpdateQuantity(CartItem item, int quantity)
        {
            item.UpdateQuantity(quantity);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            Itens.Remove(GetCartItemByProductId(item.ProductId));
            CalculateCartPrice();
        }

        internal bool IsValid()
        {
            var errors = Itens.SelectMany(i => new CartItem.CartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CustomerCartValidation().Validate(this).Errors);

            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CustomerCartValidation : AbstractValidator<CustomerCart>
        {
            public CustomerCartValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid customer");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("Itens are required");

                RuleFor(c => c.TotalPrice)
                    .GreaterThan(0)
                    .WithMessage("Total price must be greater than 0");
            }
        }
    }
}
