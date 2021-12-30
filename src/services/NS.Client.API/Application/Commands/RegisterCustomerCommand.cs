using FluentValidation;
using NS.Core.DomainObjects;
using NS.Core.Messages;
using System;

namespace NS.Customer.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegisterCustomerCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCustomerValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }

    public class RegisterCustomerValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer person id");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Empty customer name");

            RuleFor(c => c.Cpf)
                .Must(HasValidCpf)
                .WithMessage("Invalid CPF");

            RuleFor(c => c.Email)
                .Must(HasValidEmail)
                .WithMessage("Invalid e-mail");
        }

        protected static bool HasValidCpf(string cpf)
        {
            return Cpf.Validate(cpf);
        }

        protected static bool HasValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
