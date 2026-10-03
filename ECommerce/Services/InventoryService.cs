using ECommerce.Data;

namespace ECommerce.Services;

public class InventoryService
{
    private readonly AppDbContext _db;

    public InventoryService(AppDbContext db)
    {
        _db = db;
    }

    public bool Reserve(Guid productId, int quantity)
    {
        var product = _db.Products
            .FirstOrDefault(x => x.Id == productId);

        if (product == null)
            return false;

        if (product.Stock < quantity)
            return false;

        product.Stock -= quantity;

        return true;
    }
}
