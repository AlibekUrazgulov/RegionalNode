using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Database;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryUnitOfWork).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
