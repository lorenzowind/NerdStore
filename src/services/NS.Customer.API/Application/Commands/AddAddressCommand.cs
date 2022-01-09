using FluentValidation;
using NS.Core.Messages;
using System;

namespace NS.Customer.API.Application.Commands
{
    public class AddAddressCommand : Command
    {
        public Guid CustomerId { get; set; }
        public string PublicArea { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public AddAddressCommand()
        {
        }

        public AddAddressCommand(Guid customerId, string publicArea, string number, string complement, 
            string district, string zipCode, string city, string state)
        {
            AggregateId = customerId;
            CustomerId = customerId;
            PublicArea = publicArea;
            Number = number;
            Complement = complement;
            District = district;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddressValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddressValidation : AbstractValidator<AddAddressCommand>
        {
            public AddressValidation()
            {
                RuleFor(ad => ad.PublicArea)
                    .NotEmpty()
                    .WithMessage("Public area is required");
                
                RuleFor(ad => ad.Number)
                    .NotEmpty()
                    .WithMessage("Number is required");
                
                RuleFor(ad => ad.ZipCode)
                    .NotEmpty()
                    .WithMessage("Zip code is required");
                
                RuleFor(ad => ad.District)
                    .NotEmpty()
                    .WithMessage("District is required");
                
                RuleFor(ad => ad.City)
                    .NotEmpty()
                    .WithMessage("City is required");
                
                RuleFor(ad => ad.State)
                    .NotEmpty()
                    .WithMessage("State is required");
            }
        }
    }
}
