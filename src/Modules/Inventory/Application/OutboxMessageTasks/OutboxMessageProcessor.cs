namespace Inventory.Application.OutboxMessageTasks;

public class OutboxMessageProcessor(
    IOutboxMessageWeightRepository iOutboxMessageWeightRepository,
    IOutboxMessageRepository iOutboxMessageRepository) : IMessageProcessor
{

    public async Task ProcessMessagesAsync(int batchSize, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.OutboxMessageTasks.OutboxMessageWeight> outboxMessageWeights = await iOutboxMessageWeightRepository.GetItemsAsync(cancellationToken);
        if (outboxMessageWeights.Count == 0)
        {
            return;
        }

        IReadOnlyCollection<Domain.OutboxMessageTasks.OutboxMessageTask> outboxMessages = await iOutboxMessageRepository.FetchUnprocessedAsync(batchSize, cancellationToken);
        if (outboxMessages.Count == 0)
        {
            return;
        }

        Console.WriteLine($"Processed {outboxMessages.Count} outbox messages.");
    }
}
