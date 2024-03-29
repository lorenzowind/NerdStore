using Microsoft.IdentityModel.Tokens;
using NS.WebAPI.Core.Identity.Security.Jwk;

namespace NS.WebAPI.Core.Identity.Security
{
    /// <summary>
    ///  See RFC 7518 - JSON Web Algorithms (JWA) 
    /// - Section 7.1. JSON Web Signature and Encryption Algorithms Registry
    /// - Section 3.1.  "alg" (Algorithm) Header Parameter Values for JWS
    /// </summary>
    public sealed class JwsAlgorithm : Algorithm
    {
        // HMAC
        public static readonly JwsAlgorithm HS256 = new JwsAlgorithm(SecurityAlgorithms.HmacSha256, KeyType.HMAC);
        public static readonly JwsAlgorithm HS384 = new JwsAlgorithm(SecurityAlgorithms.HmacSha384, KeyType.HMAC);
        public static readonly JwsAlgorithm HS512 = new JwsAlgorithm(SecurityAlgorithms.HmacSha512, KeyType.HMAC);

        // RSA
        public static readonly JwsAlgorithm RS256 = new JwsAlgorithm(SecurityAlgorithms.RsaSha256, KeyType.RSA);
        public static readonly JwsAlgorithm RS384 = new JwsAlgorithm(SecurityAlgorithms.RsaSha384, KeyType.RSA);
        public static readonly JwsAlgorithm RS512 = new JwsAlgorithm(SecurityAlgorithms.RsaSha512, KeyType.RSA);
        public static readonly JwsAlgorithm PS256 = new JwsAlgorithm(SecurityAlgorithms.RsaSsaPssSha256, KeyType.RSA);
        public static readonly JwsAlgorithm PS384 = new JwsAlgorithm(SecurityAlgorithms.RsaSsaPssSha384, KeyType.RSA);
        public static readonly JwsAlgorithm PS512 = new JwsAlgorithm(SecurityAlgorithms.RsaSsaPssSha512, KeyType.RSA);

        // Elliptic Curve
        public static readonly JwsAlgorithm ES256 = new JwsAlgorithm(SecurityAlgorithms.EcdsaSha256, KeyType.ECDsa, JsonWebKeyECTypes.P256);
        public static readonly JwsAlgorithm ES384 = new JwsAlgorithm(SecurityAlgorithms.EcdsaSha384, KeyType.ECDsa, JsonWebKeyECTypes.P384);
        public static readonly JwsAlgorithm ES512 = new JwsAlgorithm(SecurityAlgorithms.EcdsaSha512, KeyType.ECDsa, JsonWebKeyECTypes.P521);

        // Not supported
        // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Supported-Algorithms
        // public static readonly Algorithm A192KW = new Algorithm("A192KW", KeyType.AES); -> Not supported

        private JwsAlgorithm(string alg, KeyType keyType, string curve)
        {
            Alg = alg;
            KeyType = keyType;
            Curve = curve;
        }

        private JwsAlgorithm(string alg, KeyType keyType)
        {
            this.Alg = alg;
            this.KeyType = keyType;
        }


        public static JwsAlgorithm Create(string value, KeyType key)
        {
            return new JwsAlgorithm(value, key);
        }


        public static implicit operator string(JwsAlgorithm value) => value.Alg;

    }
}