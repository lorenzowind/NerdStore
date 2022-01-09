using Microsoft.Extensions.Options;
using NS.Buyer.AGW.Extensions;
using NS.Buyer.AGW.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NS.Buyer.AGW.Services
{
    public interface ICatalogService
    {
        Task<ProductDTO> GetProductById(Guid productId);
        Task<IEnumerable<ProductDTO>> GetProducts(IEnumerable<Guid> ids);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        }

        public async Task<ProductDTO> GetProductById(Guid productId)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{productId}");

            HandleResponseErrors(response);

            return await DeserializeResponseObject<ProductDTO>(response);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/catalog/products/list/{idsRequest}");

            HandleResponseErrors(response);

            return await DeserializeResponseObject<IEnumerable<ProductDTO>>(response);
        }
    }
}
