using Inventory.Application.Abstractions.Data;

namespace Inventory.Infrastructure.Database;

public class InventoryConnectionStringProvider : IConnectionStringProvider
{
    public InventoryConnectionStringProvider(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }
}
