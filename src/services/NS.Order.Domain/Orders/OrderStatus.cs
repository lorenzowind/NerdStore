namespace NS.Order.Domain.Orders
{
    public enum OrderStatus
    {
        Authorized = 0,
        Paid = 1,
        Declined = 2,
        Delivered = 3,
        Cancelled = 4,
    }
}
