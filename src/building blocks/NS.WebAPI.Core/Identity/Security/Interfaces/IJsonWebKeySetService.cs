using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security.Interfaces
{
    public interface IJsonWebKeySetService
    {
        SigningCredentials GenerateSigningCredentials(JwksOptions options = null);
        SigningCredentials GetCurrentSigningCredentials(JwksOptions options = null);
        EncryptingCredentials GetCurrentEncryptingCredentials(JwksOptions options = null);
        EncryptingCredentials GenerateEncryptingCredentials(JwksOptions options = null);
        IReadOnlyCollection<JsonWebKey> GetLastKeysCredentials(JsonWebKeyType jsonWebKeyType, int qty);

    }
}