namespace Inventory;

public class InventoryService
{
    private readonly Data.AppDbContext _db;

    public InventoryService(Data.AppDbContext db)
    {
        _db = db;
    }

    public async Task<Product?> GetProduct(Guid productId)
    {
        return await _db.Products.FindAsync(productId);
    }

    public async Task<bool> Reserve(Guid productId, int quantity)
    {
        var product = await _db.Products.FindAsync(productId);

        if (product == null)
            return false;

        if (product.Stock < quantity)
            return false;

        product.Stock -= quantity;

        await _db.SaveChangesAsync();

        return true;
    }
}
