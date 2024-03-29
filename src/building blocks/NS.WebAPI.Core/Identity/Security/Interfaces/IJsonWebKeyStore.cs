using System.Collections.Generic;
using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security.Interfaces
{
    public interface IJsonWebKeyStore
    {
        void Save(SecurityKeyWithPrivate securityParamteres);
        SecurityKeyWithPrivate GetCurrentKey(JsonWebKeyType jwkType);
        IReadOnlyCollection<SecurityKeyWithPrivate> Get(JsonWebKeyType jwkType, int quantity = 5);
        void Clear();
        bool NeedsUpdate(JsonWebKeyType jsonWebKeyType);
        void Revoke(SecurityKeyWithPrivate securityKeyWithPrivate);
    }
}