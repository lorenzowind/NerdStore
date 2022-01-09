using Microsoft.Extensions.Options;
using NS.Buyer.AGW.Extensions;
using System;
using System.Net.Http;

namespace NS.Buyer.AGW.Services
{
    public interface IPurchaseService
    {
    }

    public class PurchaseService : Service, IPurchaseService
    {
        private readonly HttpClient _httpClient;

        public PurchaseService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PurchaseUrl);
        }
    }
}
