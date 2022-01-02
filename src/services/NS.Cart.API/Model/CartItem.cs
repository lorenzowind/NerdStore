using FluentValidation;
using System;
using System.Text.Json.Serialization;

namespace NS.Cart.API.Model
{
    public class CartItem
    {
        public Guid CartId { get; set; }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public CustomerCart CustomerCart { get; set; }

        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        internal void LinkCart(Guid cartId)
        {
            CartId = cartId;
        }

        internal decimal CalculatePrice()
        {
            return Quantity * Price;
        }

        internal void IncrementQuantity(int quantity)
        {
            Quantity += quantity;
        }

        internal void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }

        internal bool IsValid()
        {
            return new CartItemValidation().Validate(this).IsValid;
        }

        public class CartItemValidation : AbstractValidator<CartItem>
        {
            public CartItemValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid product id");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("Product name is required");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"Minimum quantity for \"{item.Name}\" is 1");

                RuleFor(c => c.Quantity)
                    .LessThanOrEqualTo(CustomerCart.MAX_QUANTITY_ITENS)
                    .WithMessage(item => $"Maximum quantity for \"{item.Name}\" is {CustomerCart.MAX_QUANTITY_ITENS}");

                RuleFor(c => c.Price)
                    .GreaterThan(0)
                    .WithMessage(item => $"Price for \"{item.Name}\" should be greater than 0");
            }
        }
    }
}
