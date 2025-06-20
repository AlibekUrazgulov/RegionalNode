using Inventory.Domain.OutboxMessageTasks;

namespace Inventory.Application.OutboxMessageTasks;

public interface IOutboxMessageWeightRepository
{
    Task<IReadOnlyCollection<OutboxMessageWeight>> GetItemsAsync(CancellationToken cancellationToken);
}
