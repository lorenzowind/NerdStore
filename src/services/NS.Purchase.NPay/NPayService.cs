namespace NS.Purchase.NPay
{
    public class NPayService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public NPayService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}