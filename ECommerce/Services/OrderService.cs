using ECommerce.Data;
using ECommerce.Models;

namespace ECommerce.Services;

public class OrderService
{
    private readonly AppDbContext _db;
    private readonly InventoryService _inventory;
    private readonly PaymentService _payment;

    public OrderService(
        AppDbContext db,
        InventoryService inventory,
        PaymentService payment)
    {
        _db = db;
        _inventory = inventory;
        _payment = payment;
    }

    public async Task<Order> CreateOrder(Guid productId, int quantity)
    {
        var product = await _db.Products.FindAsync(productId);

        if (product == null)
            throw new InvalidOperationException("Product not found");

        var amount = product.Price * quantity;

        var reserved = _inventory.Reserve(productId, quantity);

        if (!reserved)
            throw new InvalidOperationException("Insufficient stock");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = quantity,
            Amount = amount,
            Status = "Pending"
        };

        _db.Orders.Add(order);

        _payment.Process(order.Id, amount);

        order.Status = "Confirmed";

        await _db.SaveChangesAsync();

        return order;
    }
}
