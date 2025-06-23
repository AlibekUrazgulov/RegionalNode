using Inventory.SharedKernel;

namespace Inventory.Application.OutboxMessages;

public interface IOutboxMessageProcessor
{
    Task ProcessMessagesAsync(int batchSize, int displacementMinute, List<OutboxMessageType> messageTypes, CancellationToken cancellationToken);
}
