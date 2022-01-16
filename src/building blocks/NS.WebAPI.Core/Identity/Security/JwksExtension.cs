﻿using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace NS.WebAPI.Core.Identity.Security
{
    public static class JwksExtension
    {
        public static void SetJwksOptions(this JwtBearerOptions options, JwkOptions jwkOptions)
        {
            var httpClient = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler())
            {
                Timeout = options.BackchannelTimeout,
                MaxResponseContentBufferSize = 1024 * 1024 * 10 // 10 MB 
            };

            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                jwkOptions.JwksUri.OriginalString,
                new JwksRetriever(),
                new HttpDocumentRetriever(httpClient) { RequireHttps = options.RequireHttpsMetadata });
            options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters.ValidIssuer = jwkOptions.Issuer;
        }
    }
}
