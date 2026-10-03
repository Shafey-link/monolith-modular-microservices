namespace Payments;

public class PaymentService
{
    private readonly Data.AppDbContext _db;

    public PaymentService(Data.AppDbContext db)
    {
        _db = db;
    }

    public async Task ProcessPayment(Guid orderId, decimal amount)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Status = "Paid"
        };

        _db.Payments.Add(payment);

        await _db.SaveChangesAsync();
    }
}
