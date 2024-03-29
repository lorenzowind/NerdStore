using System;

namespace NS.WebAPI.Core.Identity.Security
{
    public class JwksOptions
    {
        public JwsAlgorithm Jws { get; set; } = JwsAlgorithm.ES256;
        public JweAlgorithm Jwe { get; set; } = JweAlgorithm.RsaOAEP.WithEncryption(Encryption.Aes128CbcHmacSha256);
        public int DaysUntilExpire { get; set; } = 90;
        public string KeyPrefix { get; set; } = $"{Environment.MachineName}_";
        public int AlgorithmsToKeep { get; set; } = 2;
        public TimeSpan CacheTime { get; set; } = TimeSpan.FromMinutes(15);
    }
}