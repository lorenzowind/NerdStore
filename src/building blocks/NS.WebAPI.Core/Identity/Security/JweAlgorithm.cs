using Microsoft.IdentityModel.Tokens;
using NS.WebAPI.Core.Identity.Security.Jwk;

namespace NS.WebAPI.Core.Identity.Security
{
    /// <summary>
    ///  See RFC 7518 - JSON Web Algorithms (JWA) 
    /// - Section 7.1. JSON Web Signature and Encryption Algorithms Registry
    /// - Section 4.1.  "alg" (Algorithm) Header Parameter Values for JWE
    /// - Section 5.1.  "enc" (Encryption) Header Parameter Values for JWE
    /// </summary>
    public sealed class JweAlgorithm : Algorithm
    {
        // See: https://tools.ietf.org/html/rfc7518#section-5.1
        public static readonly JweAlgorithm RsaOAEP = new JweAlgorithm(SecurityAlgorithms.RsaOAEP, KeyType.RSA);
        public static readonly JweAlgorithm RSA1_5 = new JweAlgorithm(SecurityAlgorithms.RsaPKCS1, KeyType.RSA);
        public static readonly JweAlgorithm A128KW = new JweAlgorithm(SecurityAlgorithms.Aes128KW, KeyType.AES);
        public static readonly JweAlgorithm A256KW = new JweAlgorithm(SecurityAlgorithms.Aes256KW, KeyType.AES);

        public string Encryption { get; private set; }
        private JweAlgorithm(string alg, KeyType keyType, string curve)
        {
            Alg = alg;
            KeyType = keyType;
            Curve = curve;
        }

        private JweAlgorithm(string alg, KeyType keyType)
        {
            this.Alg = alg;
            this.KeyType = keyType;
        }

        public JweAlgorithm WithEncryption(Encryption encryption)
        {
            Encryption = encryption;
            return this;
        }
        public JweAlgorithm WithEncryption(string encryption)
        {
            Encryption = encryption;
            return this;
        }

        public static JweAlgorithm Create(string alg, KeyType key)
        {
            return new JweAlgorithm(alg, key);
        }



    }
}