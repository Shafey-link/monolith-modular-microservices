using InventoryService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("InventoryDb")));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
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

app.MapGet("/products/{id:guid}", async (Guid id, InventoryDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    return product is null
        ? Results.NotFound()
        : Results.Ok(product);
});

app.MapPost("/reserve", async (ReserveRequest request, InventoryDbContext db) =>
{
    var product = await db.Products.FindAsync(request.ProductId);

    if (product == null)
        return Results.NotFound();

    if (product.Stock < request.Quantity)
        return Results.BadRequest();

    product.Stock -= request.Quantity;

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run();

public record ReserveRequest(Guid ProductId, int Quantity);
