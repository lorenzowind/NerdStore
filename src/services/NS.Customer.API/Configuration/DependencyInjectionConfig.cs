using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NS.Core.Mediator;
using NS.Customer.API.Application.Commands;
using NS.Customer.API.Application.Events;
using NS.Customer.API.Data;
using NS.Customer.API.Data.Repository;
using NS.Customer.API.Models;
using NS.WebAPI.Core.User;

namespace NS.Customer.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Commands
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<AddAddressCommand, ValidationResult>, CustomerCommandHandler>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

            // Data
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();
        }
    }
}