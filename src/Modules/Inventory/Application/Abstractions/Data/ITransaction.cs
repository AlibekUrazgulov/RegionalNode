namespace Inventory.Application.Abstractions.Data;

public interface ITransaction : IDisposable
{
    void Commit();

    void Rollback();
}
