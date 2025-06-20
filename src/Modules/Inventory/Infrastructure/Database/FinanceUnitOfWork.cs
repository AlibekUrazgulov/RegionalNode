using Inventory.Application.Abstractions.Data;
using Inventory.Application.OutboxMessageTasks;
using Inventory.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Inventory.SharedKernel;

namespace Inventory.Infrastructure.Database;

public class FinanceUnitOfWork(DbContextOptions<FinanceUnitOfWork> options,
    IDomainEventsDispatcher domainEventsDispatcher,
    IOutboxMessageWeightRepository iOutboxMessageWeightRepository,
    IOutboxMessageRepository iOutboxMessageRepository) : DbContext(options), IUnitOfWork
{
    public IOutboxMessageWeightRepository OutboxMessageWeightRepository { get; } = iOutboxMessageWeightRepository;
    public IOutboxMessageRepository OutboxMessageRepository { get; } = iOutboxMessageRepository;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceUnitOfWork).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    public async Task<long> CommitAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction transaction = await base.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(transaction);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
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
