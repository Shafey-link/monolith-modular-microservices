using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services;

public class PaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public Payment Process(Guid orderId, decimal amount)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Status = "Paid"
        };

        _db.Payments.Add(payment);

        return payment;
    }
}
