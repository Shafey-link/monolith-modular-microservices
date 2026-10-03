namespace Orders;

public static class OrderEndpoints
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services)
    {
        services.AddScoped<OrderService>();
        return services;
    }

    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
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

        return app;
    }
}

public record CreateOrderRequest(Guid ProductId, int Quantity);
