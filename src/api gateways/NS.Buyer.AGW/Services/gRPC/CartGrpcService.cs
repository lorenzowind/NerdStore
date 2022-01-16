using NS.Buyer.AGW.Models;
using NS.Cart.API.Services.gRPC;
using System;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Services.gRPC
{
    public interface ICartGrpcService
    {
        Task<CartDTO> GetCart();
    }

    public class CartGrpcService : ICartGrpcService
    {
        private readonly BuyerCart.BuyerCartClient _buyerCartClient;

        public CartGrpcService(BuyerCart.BuyerCartClient buyerCartClient)
        {
            _buyerCartClient = buyerCartClient;
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _buyerCartClient.GetCartAsync(new GetCartRequest());
            return MapCustomerCartProtoResponseToDTO(response);
        }

        private static CartDTO MapCustomerCartProtoResponseToDTO(CustomerCartResponse cartResponse)
        {
            var cartDTO = new CartDTO
            {
                TotalPrice = (decimal)cartResponse.Totalprice,
                Discount = (decimal)cartResponse.Discount,
                AppliedVoucher = cartResponse.Appliedvoucher
            };

            if (cartResponse.Voucher != null)
            {
                cartDTO.Voucher = new VoucherDTO
                {
                    Code = cartResponse.Voucher.Code,
                    Percentage = (decimal?)cartResponse.Voucher.Percentage,
                    DiscountValue = (decimal?)cartResponse.Voucher.Discountvalue,
                    DiscountType = cartResponse.Voucher.Discountype
                };
            }

            foreach (var item in cartResponse.Itens)
            {
                cartDTO.Itens.Add(new CartItemDTO
                {
                    Name = item.Name,
                    Image = item.Image,
                    ProductId = Guid.Parse(item.Productid),
                    Quantity = item.Quantity,
                    Price = (decimal)item.Price
                });
            }

            return cartDTO;
        }
    }
}