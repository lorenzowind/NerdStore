using System;
using Microsoft.Extensions.DependencyInjection;
using NS.WebAPI.Core.Identity.Security.Interfaces;

namespace NS.WebAPI.Core.Identity.Security
{
    public class JwksBuilder : IJwksBuilder
    {

        public JwksBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}