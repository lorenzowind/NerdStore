using Microsoft.Extensions.Options;
using NS.Buyer.AGW.Extensions;
using NS.Buyer.AGW.Models;
using NS.Core.Protocols;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCart();
        Task<ResponseResult> AddCartItem(CartItemDTO item);
        Task<ResponseResult> UpdateCartItem(Guid productId, CartItemDTO item);
        Task<ResponseResult> RemoveCartItem(Guid productId);
        Task<ResponseResult> ApplyCartVoucher(VoucherDTO voucher);
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            HandleResponseErrors(response);

            return await DeserializeResponseObject<CartDTO>(response);
        }

        public async Task<ResponseResult> AddCartItem(CartItemDTO item)
        {
            var itemContent = GetContent(item);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateCartItem(Guid productId, CartItemDTO item)
        {
            var itemContent = GetContent(item);

            var response = await _httpClient.PutAsync($"/cart/{productId}", itemContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveCartItem(Guid productId)
        {

            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ApplyCartVoucher(VoucherDTO voucher)
        {
            var itemContent = GetContent(voucher);

            var response = await _httpClient.PostAsync("/cart/apply-voucher", itemContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
