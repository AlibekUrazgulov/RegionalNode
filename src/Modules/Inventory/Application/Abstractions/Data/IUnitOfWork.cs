namespace Inventory.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<long> CommitAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
