using System;
using NS.Core.DomainObjects;

namespace NS.Purchase.API.Models
{
    public class Transaction : Entity
    {
        public string AuthorizationCode { get; set; }
        public string CardBrand { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public TransactionStatus Status { get; set; }
        public string TID { get; set; }
        public string NSU { get; set; }

        public Guid PurchaseId { get; set; }

        public Purchase Purchase { get; set; }
    }
}