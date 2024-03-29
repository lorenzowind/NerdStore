using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security.Jwks
{
    public class JwkContants
    {
        public static string CurrentJwkCache(JsonWebKeyType jwkType) => $"NS-CURRENT-{jwkType}-SECURITY-KEY";
        public static string JwksCache(JsonWebKeyType jwkType) => $"NS-JWKS-{jwkType}";
    }
}