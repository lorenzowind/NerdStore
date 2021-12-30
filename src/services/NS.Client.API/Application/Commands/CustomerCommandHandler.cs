using FluentValidation.Results;
using MediatR;
using NS.Core.Messages;
using NS.Customer.API.Application.Events;
using NS.Customer.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NS.Customer.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new CustomerPerson(message.Id, message.Name, message.Email, message.Cpf);

            var existingCustomer = await _customerRepository.GetByCpf(customer.Cpf.Number);

            if (existingCustomer != null)
            {
                AddError("This CPF is already used");

                return ValidationResult;
            }

            _customerRepository.Add(customer);

            customer.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistData(_customerRepository.UnitOfWork);
        }
    }
}
