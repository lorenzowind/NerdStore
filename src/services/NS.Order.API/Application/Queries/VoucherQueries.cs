using NS.Order.API.Application.DTO;
using NS.Order.Domain.Vouchers;
using System.Threading.Tasks;

namespace NS.Order.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDTO> GetVoucherByCode(string code);
    }

    public class VoucherQueries : IVoucherQueries
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);

            if (voucher == null) return null;

            if (!voucher.IsValid()) return null;

            return new VoucherDTO
            {
                Code = voucher.Code,
                DiscountType = (int)voucher.DiscountType,
                DiscountValue = voucher.DiscountValue,
                Percentage = voucher.Percentage
            };
        }
    }
}
