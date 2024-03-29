using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NS.WebAPI.Core.Identity.Security.Models
{
    [DebuggerDisplay("{JwkType}-{Type}")]
    public class SecurityKeyWithPrivate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Parameters { get; set; }
        public string KeyId { get; set; }
        public string Type { get; set; }
        public string JwsAlgorithm { get; set; }
        public string JweAlgorithm { get; set; }
        public string JweEncryption { get; set; }
        public DateTime CreationDate { get; set; }
        public JsonWebKeyType JwkType { get; set; }
        public bool IsRevoked { get; set; }
        public void SetJwsParameters(SecurityKey key, JwsAlgorithm alg)
        {
            Parameters = JsonSerializer.Serialize(key, typeof(JsonWebKey), new JsonSerializerOptions() { IgnoreNullValues = true, });
            Type = alg.Kty();
            KeyId = key.KeyId;
            JwsAlgorithm = alg;
            CreationDate = DateTime.Now;
            JwkType = JsonWebKeyType.Jws;
        }
        public void SetJweParameters(SecurityKey key, JweAlgorithm alg)
        {
            Parameters = JsonSerializer.Serialize(key, typeof(JsonWebKey), new JsonSerializerOptions() { IgnoreNullValues = true, });
            Type = alg.Kty();
            KeyId = key.KeyId;

            JweAlgorithm = alg.Alg;
            JweEncryption = alg.Encryption;
            CreationDate = DateTime.Now;
            JwkType = JsonWebKeyType.Jwe;
        }

        public JsonWebKey GetSecurityKey()
        {
            return JsonSerializer.Deserialize<JsonWebKey>(Parameters);
        }

        public SigningCredentials GetSigningCredentials(JwksOptions jwksOptions)
        {
            return new SigningCredentials(GetSecurityKey(), jwksOptions.Jws);
        }

        public EncryptingCredentials GetEncryptingCredentials(JwksOptions jwksOptions)
        {
            return new EncryptingCredentials(GetSecurityKey(), jwksOptions.Jwe, jwksOptions.Jwe.Encryption);
        }

        public void Revoke()
        {
            var jsonWebKey = GetSecurityKey();
            var publicWebKey = PublicJsonWebKey.FromJwk(jsonWebKey);
            IsRevoked = true;
            Parameters = JsonSerializer.Serialize(publicWebKey.ToNativeJwk(), new JsonSerializerOptions() { IgnoreNullValues = true });
        }


        public bool IsExpired(int valueDaysUntilExpire)
        {
            return CreationDate.AddDays(valueDaysUntilExpire) < DateTime.Now;
        }
    }
}