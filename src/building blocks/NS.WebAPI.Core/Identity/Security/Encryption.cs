using Microsoft.IdentityModel.Tokens;

namespace NS.WebAPI.Core.Identity.Security
{
    /// <summary>
    ///  See RFC 7518 - JSON Web Algorithms (JWA) 
    /// - Section 7.1. JSON Web Signature and Encryption Algorithms Registry
    /// - Section 4.1.  "alg" (Algorithm) Header Parameter Values for JWS
    /// </summary>
    public sealed class Encryption
    {
        // HMAC
        public static readonly Encryption Aes128CbcHmacSha256 = new Encryption(SecurityAlgorithms.Aes128CbcHmacSha256);
        public static readonly Encryption Aes192CbcHmacSha384 = new Encryption(SecurityAlgorithms.Aes192CbcHmacSha384);
        public static readonly Encryption Aes256CbcHmacSha512 = new Encryption(SecurityAlgorithms.Aes256CbcHmacSha512);

        public string Enc { get; }

        private Encryption(string enc)
        {
            this.Enc = enc;
        }

        public static Encryption Create(string alg)
        {
            return new Encryption(alg);
        }

        public static implicit operator string(Encryption value) => value.Enc;

    }
}