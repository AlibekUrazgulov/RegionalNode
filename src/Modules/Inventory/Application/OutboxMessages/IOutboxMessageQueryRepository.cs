using Inventory.SharedKernel;

namespace Inventory.Application.OutboxMessages;

public interface IOutboxMessageQueryRepository
{
    Task<List<OutboxMessageTaskOutput>> FetchUnprocessedAsync(OutboxMessageType messageType, int batchSize, int displacementMinute, CancellationToken cancellationToken);
    Task<List<OutboxMessageTaskOutput>> FetchUnprocessedAsync(List<OutboxMessageType> messageTypes, int batchSize, int displacementMinute, CancellationToken cancellationToken);
}
