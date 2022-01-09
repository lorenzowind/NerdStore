using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using NS.Core.DomainObjects;
using NS.Core.Messages.Integration;
using NS.Purchase.API.Facade;
using NS.Purchase.API.Models;

namespace NS.Purchase.API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseFacade _purchaseFacade;
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IPurchaseFacade purchaseFacade,
                                IPurchaseRepository purchaseRepository)
        {
            _purchaseFacade = purchaseFacade;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<ResponseMessage> AuthorizePurchase(Models.Purchase purchase)
        {
            var transaction = await _purchaseFacade.AuthorizePurchase(purchase);
            var validationResult = new ValidationResult();

            if (transaction.Status != TransactionStatus.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase", 
                    "Purchase declined, contact your card provider"));

                return new ResponseMessage(validationResult);
            }

            purchase.AddTransaction(transaction);
            _purchaseRepository.AddPurchase(purchase);

            if (!await _purchaseRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase",
                    "An error occurred to conclude the purchase"));

                await CancelPurchase(purchase.OrderId);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> InterceptPurchase(Guid orderId)
        {
            var transactions = await _purchaseRepository.GetTransactionsByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction == null) throw new DomainException($"Transaction not found to order {orderId}");

            var transaction =  await _purchaseFacade.InterceptPurchase(authorizedTransaction);

            if (transaction.Status != TransactionStatus.Paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase",
                    $"It was not possible to intercept the purchase of order {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PurchaseId = authorizedTransaction.PurchaseId;
            _purchaseRepository.AddTransaction(transaction);

            if (!await _purchaseRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase",
                    $"It was not possible to persist the purchase interception of order {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelPurchase(Guid orderId)
        {
            var transactions = await _purchaseRepository.GetTransactionsByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction == null) throw new DomainException($"Transaction not found to order {orderId}");

            var transaction = await _purchaseFacade.CancelAuthorization(authorizedTransaction);

            if (transaction.Status != TransactionStatus.Cancelled)
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase",
                    $"It was not possible to cancel the purchase of order {orderId}"));

                return new ResponseMessage(validationResult);
            }

            transaction.PurchaseId = authorizedTransaction.PurchaseId;
            _purchaseRepository.AddTransaction(transaction);

            if (!await _purchaseRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Purchase",
                    $"It was not possible to persist the purchase cancelation of order {orderId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}