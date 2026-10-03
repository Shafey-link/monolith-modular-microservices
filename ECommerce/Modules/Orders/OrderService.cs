namespace Orders;

public class OrderService
{
    private readonly Data.AppDbContext _db;
    private readonly Inventory.InventoryService _inventory;
    private readonly Payments.PaymentService _payments;

    public OrderService(
        Data.AppDbContext db,
        Inventory.InventoryService inventory,
        Payments.PaymentService payments)
    {
        _db = db;
        _inventory = inventory;
        _payments = payments;
    }

    public async Task<Order> CreateOrder(Guid productId, int quantity)
    {
        var product = await _inventory.GetProduct(productId);

        if (product == null)
            throw new InvalidOperationException("Product not found");

        var amount = product.Price * quantity;

        var reserved = await _inventory.Reserve(productId, quantity);

        if (!reserved)
            throw new InvalidOperationException("Insufficient stock");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = quantity,
            Amount = amount
        };

        _db.Orders.Add(order);

        await _payments.ProcessPayment(order.Id, amount);

        order.Status = "Confirmed";

        await _db.SaveChangesAsync();

        return order;
    }
}
