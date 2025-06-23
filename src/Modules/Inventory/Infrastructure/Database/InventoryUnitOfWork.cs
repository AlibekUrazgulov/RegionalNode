using Inventory.Application.Abstractions.Data;
using Inventory.Application.OutboxMessages;
using Inventory.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore.Storage;
using Inventory.SharedKernel;

namespace Inventory.Infrastructure.Database;

public class InventoryUnitOfWork(InventoryDbContext dbContext,
    IDomainEventsDispatcher domainEventsDispatcher,
    IOutboxMessageQueryRepository iOutboxMessageQueryRepository) : IUnitOfWork
{
    public IOutboxMessageQueryRepository OutboxMessageQueryRepository { get; } = iOutboxMessageQueryRepository;

    public async Task<long> CommitAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync();

        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(transaction);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
