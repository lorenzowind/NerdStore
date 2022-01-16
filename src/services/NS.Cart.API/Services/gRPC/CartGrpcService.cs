using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NS.Cart.API.Data;
using NS.Cart.API.Models;
using NS.Cart.API.Services.gRPC;
using NS.WebAPI.Core.User;

namespace NSE.cart.API.Services.gRPC
{
    [Authorize]
    public class CartGrpcService : BuyerCart.BuyerCartBase
    {
        private readonly ILogger<CartGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly CartContext _context;

        public CartGrpcService(
            ILogger<CartGrpcService> logger,
            IAspNetUser user,
            CartContext context)
        {
            _logger = logger;
            _user = user;
            _context = context;
        }

        public override async Task<CustomerCartResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Calling GetCustomerCart");

            var cart = await GetCustomerCart() ?? new CustomerCart();

            return MapCustomerCartToProtoResponse(cart);
        }

        private async Task<CustomerCart> GetCustomerCart()
        {
            return await _context.CustomerCarts
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private static CustomerCartResponse MapCustomerCartToProtoResponse(CustomerCart cart)
        {
            var protoCart = new CustomerCartResponse
            {
                Id = cart.Id.ToString(),
                Customerid = cart.CustomerId.ToString(),
                Totalprice = (double)cart.TotalPrice,
                Discount = (double)cart.Discount,
                Appliedvoucher = cart.AppliedVoucher,
            };

            if (cart.Voucher != null)
            {
                protoCart.Voucher = new VoucherResponse
                {
                    Code = cart.Voucher.Code,
                    Percentage = (double?)cart.Voucher.Percentage ?? 0,
                    Discountvalue = (double?)cart.Voucher.DiscountValue ?? 0,
                    Discountype = (int)cart.Voucher.DiscountType
                };
            }

            foreach (var item in cart.Itens)
            {
                protoCart.Itens.Add(new CartItemResponse
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Productid = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                    Price = (double)item.Price
                });
            }

            return protoCart;
        }
    }
}