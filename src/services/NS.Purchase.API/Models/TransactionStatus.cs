namespace NS.Purchase.API.Models
{
    public enum TransactionStatus
    {
        Authorized = 1,
        Paid,
        Declined,
        Chargedback,
        Cancelled
    }
}