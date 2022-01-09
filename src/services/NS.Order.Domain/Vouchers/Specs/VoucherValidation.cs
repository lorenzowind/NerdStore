using NS.Core.Specification.Validation;

namespace NS.Order.Domain.Vouchers.Specs
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            var dateSpec = new VoucherDateSpecification();
            var qtySpec = new VoucherQuantitySpecification();
            var activeSpec = new VoucherActiveSpecification();

            Add("dateSpec", new Rule<Voucher>(dateSpec, "Voucher expired"));
            Add("qtySpec", new Rule<Voucher>(qtySpec, "Voucher already used"));
            Add("activeSpec", new Rule<Voucher>(activeSpec, "Voucher not active anymore"));
        }
    }
}
