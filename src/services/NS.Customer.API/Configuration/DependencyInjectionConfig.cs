using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NS.Core.Mediator;
using NS.Customer.API.Application.Commands;
using NS.Customer.API.Application.Events;
using NS.Customer.API.Data;
using NS.Customer.API.Data.Repository;
using NS.Customer.API.Models;

namespace NS.Customer.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();

            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();
        }
    }
}