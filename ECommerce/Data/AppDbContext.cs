using Inventory;
using Microsoft.EntityFrameworkCore;
using Orders;
using Payments;

namespace Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Payment> Payments => Set<Payment>();
}
