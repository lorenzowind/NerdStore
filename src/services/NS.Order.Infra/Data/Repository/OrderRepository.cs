using Microsoft.EntityFrameworkCore;
using NS.Core.Data;
using NS.Order.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NS.Order.Infra.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public async Task<Domain.Orders.Order> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Orders.Order>> GetListByCustomerId(Guid customerId)
        {
            return await _context.Orders
                .Include(p => p.OrderItens)
                .AsNoTracking()
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<OrderItem> GetItemById(Guid id)
        {
            return await _context.OrderItens.FindAsync(id);
        }

        public async Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.OrderItens
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);
        }

        public void Add(Domain.Orders.Order order)
        {
            _context.Orders.Add(order);
        }

        public void Update(Domain.Orders.Order order)
        {
            _context.Orders.Update(order);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
