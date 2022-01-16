using Microsoft.Extensions.DependencyInjection;

namespace NS.WebAPI.Core.Identity.Security.Interfaces
{
    public interface IJwksBuilder
    {
        IServiceCollection Services { get; }
    }
}