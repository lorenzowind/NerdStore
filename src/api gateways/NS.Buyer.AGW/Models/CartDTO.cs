using System.Collections.Generic;

namespace NS.Buyer.AGW.Models
{
    public class CartDTO
    {
        public decimal TotalPrice { get; set; }
        public VoucherDTO Voucher { get; set; }
        public bool AppliedVoucher { get; set; }
        public decimal Discount { get; set; }
        public List<CartItemDTO> Itens { get; set; } = new List<CartItemDTO>();
    }
}
