using Microsoft.IdentityModel.Tokens;

namespace NS.WebAPI.Core.Identity.Security.Interfaces
{
    public interface IJsonWebKeyService
    {
        JsonWebKey Generate(Algorithm algorithm);
    }
}
