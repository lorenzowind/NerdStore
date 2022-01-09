﻿using Microsoft.EntityFrameworkCore;
using NS.Customer.API.Models;
using NS.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using NS.Core.Mediator;
using FluentValidation.Results;
using NS.Core.Messages;
using NS.Core.DomainObjects;

namespace NS.Customer.API.Data
{
    public class CustomerContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerContext(DbContextOptions<CustomerContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in
                modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            foreach (var relationship in
                modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys())) 
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;

            if (success) await _mediatorHandler.PublishEvents(this);

            return success;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList().ForEach(e => e.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
