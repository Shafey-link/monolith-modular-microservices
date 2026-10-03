using Data;
using Inventory;
using Microsoft.EntityFrameworkCore;
using Orders;
using Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddOrderModule();
builder.Services.AddPaymentModule();
builder.Services.AddInventoryModule();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (!await db.Products.AnyAsync())
    {
        db.Products.Add(new Product
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Coffee",
            Price = 4.50m,
            Stock = 20
        });

        await db.SaveChangesAsync();
    }
}

app.MapOrderEndpoints();
app.MapPaymentEndpoints();
app.MapInventoryEndpoints();

app.Run();
