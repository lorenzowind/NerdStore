using Microsoft.Extensions.Options;
using NS.Buyer.AGW.Extensions;
using NS.Buyer.AGW.Models;
using NS.Core.Protocols;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Services
{
    public interface IOrderService
    {
        Task<ResponseResult> ConcludeOrder(OrderDTO order);
        Task<OrderDTO> GetLastOrder();
        Task<IEnumerable<OrderDTO>> GetListByCustomerId();
        Task<VoucherDTO> GetVoucherByCode(string code);
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
        }

        public async Task<ResponseResult> ConcludeOrder(OrderDTO order)
        {
            var orderContent = GetContent(order);

            var response = await _httpClient.PostAsync("/order/", orderContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<OrderDTO> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("/order/last/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandleResponseErrors(response);

            return await DeserializeResponseObject<OrderDTO>(response);
        }

        public async Task<IEnumerable<OrderDTO>> GetListByCustomerId()
        {
            var response = await _httpClient.GetAsync("/order/list-customer/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandleResponseErrors(response);

            return await DeserializeResponseObject<IEnumerable<OrderDTO>>(response);
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var response = await _httpClient.GetAsync($"/voucher/{code}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandleResponseErrors(response);

            return await DeserializeResponseObject<VoucherDTO>(response);
        }
    }
}
