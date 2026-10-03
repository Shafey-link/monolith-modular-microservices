using Microsoft.EntityFrameworkCore;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("OrderDb")));

builder.Services.AddHttpClient();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapPost("/orders", async (
    CreateOrderRequest request,
    OrderDbContext db,
    IHttpClientFactory httpClientFactory,
    IConfiguration config) =>
{
    var inventoryBase = config["Services:Inventory"];
    var paymentBase = config["Services:Payment"];
    var client = httpClientFactory.CreateClient();

    try
    {
        var productResponse = await client.GetAsync(
            $"{inventoryBase}/products/{request.ProductId}");

        if (!productResponse.IsSuccessStatusCode)
            return Results.BadRequest("Product not found");

        var product = await productResponse.Content.ReadFromJsonAsync<Product>();

        if (product is null)
            return Results.BadRequest("Product not found");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Amount = product.Price * request.Quantity,
            Status = "Pending"
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        var inventoryResponse = await client.PostAsJsonAsync(
            $"{inventoryBase}/reserve",
            new
            {
                productId = request.ProductId,
                quantity = request.Quantity
            });

        if (!inventoryResponse.IsSuccessStatusCode)
            return Results.BadRequest("Insufficient inventory");

        var paymentResponse = await client.PostAsJsonAsync(
            $"{paymentBase}/payments",
            new
            {
                orderId = order.Id,
                amount = order.Amount
            });

        if (!paymentResponse.IsSuccessStatusCode)
            return Results.BadRequest("Payment failed");

        order.Status = "Confirmed";
        await db.SaveChangesAsync();

        return Results.Ok(order);
    }
    catch (HttpRequestException)
    {
        return Results.Problem("A downstream service could not be reached.");
    }
});

app.Run();

public record CreateOrderRequest(Guid ProductId, int Quantity);

public record Product(Guid Id, string Name, decimal Price, int Stock);
