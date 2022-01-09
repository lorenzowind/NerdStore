using NS.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace NS.Purchase.API.Models
{
    public class Purchase : Entity, IAggregateRoot
    {
        public Purchase()
        {
            Transactions = new List<Transaction>();
        }

        public Guid OrderId { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Price { get; set; }

        public CreditCard CreditCard { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}