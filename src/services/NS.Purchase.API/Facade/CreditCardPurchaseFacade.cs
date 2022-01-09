using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NS.Purchase.NPay;

namespace NS.Purchase.API.Facade
{
    public class CreditCardPurchaseFacade : IPurchaseFacade
    {
        private readonly PurchaseConfig _purchaseConfig;

        public CreditCardPurchaseFacade(IOptions<PurchaseConfig> purchaseConfig)
        {
            _purchaseConfig = purchaseConfig.Value;
        }

        public async Task<Models.Transaction> AuthorizePurchase(Models.Purchase purchase)
        {
            var nPayService = new NPayService(_purchaseConfig.DefaultApiKey,
                _purchaseConfig.DefaultEncryptionKey);

            var cardHashGen = new CardHash(nPayService)
            {
                CardNumber = purchase.CreditCard.CardName,
                CardHolderName = purchase.CreditCard.CardNumber,
                CardExpirationDate = purchase.CreditCard.ExpirationMonthYear,
                CardCvv = purchase.CreditCard.CVV
            };
            var cardHash = cardHashGen.Generate();

            var transaction = new Transaction(nPayService)
            {
                CardHash = cardHash,
                CardNumber = purchase.CreditCard.CardNumber,
                CardHolderName = purchase.CreditCard.CardName,
                CardExpirationDate = purchase.CreditCard.ExpirationMonthYear,
                CardCvv = purchase.CreditCard.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = purchase.Price
            };

            return ToTransaction(await transaction.AuthorizeCardTransaction());
        }

        public async Task<Models.Transaction> InterceptPurchase(Models.Transaction transaction)
        {
            var nPayService = new NPayService(_purchaseConfig.DefaultApiKey,
                _purchaseConfig.DefaultEncryptionKey);

            var nPayTransaction = ToTransaction(transaction, nPayService);

            return ToTransaction(await nPayTransaction.CaptureCardTransaction());
        }

        public async Task<Models.Transaction> CancelAuthorization(Models.Transaction transaction)
        {
            var nPayService = new NPayService(_purchaseConfig.DefaultApiKey,
                _purchaseConfig.DefaultEncryptionKey);

            var nPayTransaction = ToTransaction(transaction, nPayService);

            return ToTransaction(await nPayTransaction.CancelAuthorization());
        }

        public static Models.Transaction ToTransaction(Transaction transaction)
        {
            return new Models.Transaction
            {
                Id = Guid.NewGuid(),
                Status = (Models.TransactionStatus) transaction.Status,
                Amount = transaction.Amount,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.Cost,
                TransactionDate = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }

        public static Transaction ToTransaction(Models.Transaction transaction, NPayService nPayService)
        {
            return new Transaction(nPayService)
            {
                Status = (TransactionStatus)transaction.Status,
                Amount = transaction.Amount,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.Cost,
                Nsu = transaction.NSU,
                Tid = transaction.TID
            };
        }
    }
}