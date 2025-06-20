namespace Inventory.Application.OutboxMessageTasks;

public interface IMessageProcessor
{
    Task ProcessMessagesAsync(int batchSize, CancellationToken cancellationToken);
}
