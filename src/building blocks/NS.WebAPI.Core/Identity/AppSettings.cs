namespace NS.WebAPI.Core.Identity
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int HoursExpiration { get; set; }
        public string Issuer { get; set; }
        public string ValidAt { get; set; }
    }
}
