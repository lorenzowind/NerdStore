using System;

namespace NS.Buyer.AGW.Models
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int StorageQuantity { get; set; }
    }
}
