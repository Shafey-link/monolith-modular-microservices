using Microsoft.EntityFrameworkCore;
using PaymentService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PaymentDb")));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapPost("/payments", async (PaymentRequest request, PaymentDbContext db) =>
{
    var payment = new Payment
    {
        Id = Guid.NewGuid(),
        OrderId = request.OrderId,
        Amount = request.Amount,
        Status = "Paid"
    };

    db.Payments.Add(payment);

    await db.SaveChangesAsync();

    return Results.Ok(payment);
});

app.Run();

public record PaymentRequest(Guid OrderId, decimal Amount);
