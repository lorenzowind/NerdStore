using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NS.Core.Mediator;
using NS.Order.API.Application.Commands;
using NS.Order.API.Application.Events;
using NS.Order.API.Application.Queries;
using NS.Order.Domain.Orders;
using NS.Order.Domain.Vouchers;
using NS.Order.Infra.Data;
using NS.Order.Infra.Data.Repository;
using NS.WebAPI.Core.User;

namespace NS.Order.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Commands
            services.AddScoped<IRequestHandler<AddOrderCommand, ValidationResult>, OrderCommandHandler>();

            // Events
            services.AddScoped<INotificationHandler<SucceedOrderEvent>, SucceedOrderHandler>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IOrderQueries, OrderQueries>();

            // Data
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<OrderContext>();
        }
    }
}
