namespace NS.Cart.API.Models
{
    public enum DiscountVoucherType
    {
        Percentage = 0,
        Value = 1
    }

    public class Voucher
    {
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public string Code { get; set; }
        public DiscountVoucherType DiscountType { get; set; }
    }
}
