namespace Payments;

public static class PaymentEndpoints
{
    public static IServiceCollection AddPaymentModule(this IServiceCollection services)
    {
        services.AddScoped<PaymentService>();
        return services;
    }

    public static WebApplication MapPaymentEndpoints(this WebApplication app)
    {
        // Payments stay inside the process. Orders calls PaymentService directly.
        return app;
    }
}
