namespace Inventory;

public static class InventoryEndpoints
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        services.AddScoped<InventoryService>();
        return services;
    }

    public static WebApplication MapInventoryEndpoints(this WebApplication app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, InventoryService inventory) =>
        {
            var product = await inventory.GetProduct(id);

            return product is null
                ? Results.NotFound()
                : Results.Ok(product);
        });

        return app;
    }
}
