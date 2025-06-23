namespace Inventory.Application.Abstractions.Data;

public interface IConnectionStringProvider
{
    string ConnectionString { get; }
}
