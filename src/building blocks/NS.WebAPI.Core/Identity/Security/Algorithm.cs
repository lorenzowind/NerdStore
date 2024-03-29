using System;
using Microsoft.IdentityModel.Tokens;
using NS.WebAPI.Core.Identity.Security.Jwk;

namespace NS.WebAPI.Core.Identity.Security
{
    public abstract class Algorithm
    {
        public KeyType KeyType { get; internal set; }
        public string Curve { get; internal set; }
        public string Alg { get; internal set; }


        /// <summary>
        /// See RFC 7518 - JSON Web Algorithms (JWA) - Section 6.1. "kty" (Key Type) Parameter Values
        /// </summary>
        public string Kty()
        {
            return KeyType switch
            {
                KeyType.RSA => JsonWebAlgorithmsKeyTypes.RSA,
                KeyType.ECDsa => JsonWebAlgorithmsKeyTypes.EllipticCurve,
                KeyType.HMAC => JsonWebAlgorithmsKeyTypes.Octet,
                KeyType.AES => JsonWebAlgorithmsKeyTypes.Octet,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public static implicit operator string(Algorithm value) => value.Alg;
    }
}