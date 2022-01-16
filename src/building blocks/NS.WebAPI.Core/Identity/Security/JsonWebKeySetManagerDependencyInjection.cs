using Microsoft.Extensions.DependencyInjection;
using NS.WebAPI.Core.Identity.Security.DefaultStore;
using NS.WebAPI.Core.Identity.Security.DefaultStore.Memory;
using NS.WebAPI.Core.Identity.Security.Interfaces;
using NS.WebAPI.Core.Identity.Security.Jwk;
using NS.WebAPI.Core.Identity.Security.Jwks;
using System;

namespace NS.WebAPI.Core.Identity.Security
{
    public static class JsonWebKeySetManagerDependencyInjection
    {
        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <returns></returns>
        public static IJwksBuilder AddJwksManager(this IServiceCollection services, Action<JwksOptions> action = null)
        {
            if (action != null)
                services.Configure(action);

            services.AddDataProtection();
            services.AddScoped<IJsonWebKeyService, JwkService>();
            services.AddScoped<IJsonWebKeySetService, JwksService>();
            services.AddSingleton<IJsonWebKeyStore, DataProtectionStore>();

            return new JwksBuilder(services);
        }

        /// <summary>
        /// Sets the signing credential.
        /// </summary>
        /// <returns></returns>
        public static IJwksBuilder PersistKeysInMemory(this IJwksBuilder builder)
        {
            builder.Services.AddSingleton<IJsonWebKeyStore, InMemoryStore>();

            return builder;
        }
    }
}
