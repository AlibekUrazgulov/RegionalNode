using Inventory.Domain.OutboxMessageTasks;

namespace Inventory.Application.OutboxMessageTasks;

public interface IOutboxMessageRepository
{
    Task<IReadOnlyCollection<OutboxMessageTask>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken);
}
