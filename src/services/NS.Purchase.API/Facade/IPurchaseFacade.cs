using NS.Purchase.API.Models;
using System.Threading.Tasks;

namespace NS.Purchase.API.Facade
{
    public interface IPurchaseFacade
    {
        Task<Transaction> AuthorizePurchase(Models.Purchase purchase);
        Task<Transaction> InterceptPurchase(Transaction transaction);
        Task<Transaction> CancelAuthorization(Transaction transaction);
    }
}