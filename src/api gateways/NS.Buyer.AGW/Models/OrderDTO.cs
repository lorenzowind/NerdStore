using NS.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NS.Buyer.AGW.Models
{
    public class OrderDTO
    {
        public int Code { get; set; }
        // Authorized = 1,
        // Paid = 2,
        // Declined = 3,
        // Delivered = 4,
        // Cancelled = 5
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool AppliedVoucher { get; set; }

        public List<CartItemDTO> OrderItens { get; set; }

        public AddressDTO Address { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card name is required")]
        [DisplayName("Card Name")]
        public string CardName { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration must be in format MM/YY")]
        [CardExpiration(ErrorMessage = "Expired Card")]
        [Required(ErrorMessage = "Expiration card is required")]
        [DisplayName("Expiration Date MM/YY")]
        public string CardExpiration { get; set; }

        [Required(ErrorMessage = "Security code is required")]
        [DisplayName("Security Code")]
        public string CardCvv { get; set; }
    }
}
