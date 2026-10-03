using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<InventoryService>();

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

app.MapPost("/orders", async (CreateOrderRequest request, OrderService orders) =>
{
    try
    {
        var order = await orders.CreateOrder(request.ProductId, request.Quantity);
        return Results.Ok(order);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

public record CreateOrderRequest(Guid ProductId, int Quantity);
