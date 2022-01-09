using NS.Core.Messages.Integration;
using System;
using System.Threading.Tasks;

namespace NS.Purchase.API.Services
{
    public interface IPurchaseService
    {
        Task<ResponseMessage> AuthorizePurchase(Models.Purchase purchase);
        Task<ResponseMessage> InterceptPurchase(Guid orderId);
        Task<ResponseMessage> CancelPurchase(Guid orderId);
    }
}