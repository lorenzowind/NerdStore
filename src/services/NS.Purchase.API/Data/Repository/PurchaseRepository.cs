using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NS.Core.Data;
using NS.Purchase.API.Models;

namespace NS.Purchase.API.Data.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly PurchaseContext _context;

        public PurchaseRepository(PurchaseContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AddPurchase(Models.Purchase purchase)
        {
            _context.Purchases.Add(purchase);
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public async Task<Models.Purchase> GetPurchaseByOrderId(Guid orderId)
        {
            return await _context.Purchases.AsNoTracking()
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId)
        {
            return await _context.Transactions.AsNoTracking()
                .Where(t => t.Purchase.OrderId == orderId).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}