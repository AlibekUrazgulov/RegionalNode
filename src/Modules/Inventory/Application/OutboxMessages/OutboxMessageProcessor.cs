using Inventory.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.OutboxMessages;

public class OutboxMessageProcessor(IOutboxMessageQueryRepository queryRepository,
    IServiceProvider serviceProvider,
    ILogger<OutboxMessageProcessor> logger) : IOutboxMessageProcessor
{
    public async Task ProcessMessagesAsync(int batchSize, int displacementMinute, List<OutboxMessageType> messageTypes, CancellationToken cancellationToken)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than zero.");
        }

        if (displacementMinute <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(displacementMinute), "Displacement minute must be greater than zero.");
        }

        List<OutboxMessageTaskOutput> messages = await queryRepository.FetchUnprocessedAsync(messageTypes, batchSize, displacementMinute, cancellationToken);

        foreach (OutboxMessageTaskOutput message in messages)
        {
            IOutboxMessageHandler? handler =
                serviceProvider.GetKeyedService<IOutboxMessageHandler>(message.MessageType);
            if (handler is null)
            {
                logger.LogError("No handler found for messageTypeName {MessageTypeName}", message.MessageType);
                continue;
            }
            await handler.HandleAsync(message, cancellationToken);
        }
    }
}
