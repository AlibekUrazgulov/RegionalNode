namespace Inventory.Application.OutboxMessages;

public interface IOutboxMessageProcessorFacade
{
    Task ProcessMessagesAsync(int batchSize, int displacementMinute, CancellationToken cancellationToken);
}
