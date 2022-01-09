using FluentValidation;
using NS.Core.Messages;
using NS.Order.API.Application.DTO;
using System;
using System.Collections.Generic;

namespace NS.Order.API.Application.Commands
{
    public class AddOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItens { get; set; }

        public string VoucherCode { get; set; }
        public bool AppliedVoucher { get; set; }
        public decimal Discount { get; set; }

        public AddressDTO Address { get; set; }

        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardExpiration { get; set; }
        public string CardCvv { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddOrderValidation : AbstractValidator<AddOrderCommand>
        {
            public AddOrderValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid customer Id");

                RuleFor(c => c.OrderItens.Count)
                    .GreaterThan(0)
                    .WithMessage("Order must have 1 item or more");

                RuleFor(c => c.TotalPrice)
                    .GreaterThan(0)
                    .WithMessage("Invalid order price");

                RuleFor(c => c.CardNumber)
                    .CreditCard()
                    .WithMessage("Invalid card number");

                RuleFor(c => c.CardName)
                    .NotNull()
                    .WithMessage("Card name is required");

                RuleFor(c => c.CardCvv.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("Card CVV must have 3 or 4 numbers");

                RuleFor(c => c.CardExpiration)
                    .NotNull()
                    .WithMessage("Card expiration date is required");
            }
        }
    }
}
