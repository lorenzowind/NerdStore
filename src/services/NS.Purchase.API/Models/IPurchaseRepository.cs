using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NS.Core.Data;

namespace NS.Purchase.API.Models
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        void AddPurchase(Purchase purchase);
        void AddTransaction(Transaction transaction);
        Task<Purchase> GetPurchaseByOrderId(Guid orderId);
        Task<IEnumerable<Transaction>> GetTransactionsByOrderId(Guid orderId);
    }
}