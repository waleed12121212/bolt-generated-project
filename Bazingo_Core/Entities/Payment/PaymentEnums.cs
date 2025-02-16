namespace Bazingo_Core.Entities.Payment
{
    public enum PaymentMethod
    {
        CreditCard = 1,
        DebitCard = 2,
        BankTransfer = 3,
        PayPal = 4,
        Crypto = 5
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Authorized = 2,
        Paid = 3,
        Completed = 4,
        Refunded = 5,
        Failed = 6
    }
}
