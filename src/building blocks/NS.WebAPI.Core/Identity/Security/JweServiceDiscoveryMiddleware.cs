using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NS.WebAPI.Core.Identity.Security.Interfaces;
using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security
{
    public class JweServiceDiscoveryMiddleware
    {
        private readonly RequestDelegate _next;

        public JweServiceDiscoveryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IJsonWebKeySetService keyService, IOptions<JwksOptions> options)
        {
            var keys = new
            {
                keys = keyService.GetLastKeysCredentials(JsonWebKeyType.Jwe, options.Value.AlgorithmsToKeep)?.Select(PublicJsonWebKey.FromJwk)
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(keys, new JsonSerializerOptions() { IgnoreNullValues = true }));
        }
    }
}