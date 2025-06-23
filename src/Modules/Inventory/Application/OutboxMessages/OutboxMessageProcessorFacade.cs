using Inventory.SharedKernel;

namespace Inventory.Application.OutboxMessages;

public class OutboxMessageProcessorFacade(IOutboxMessageProcessor iOutboxMessageProcessor) : IOutboxMessageProcessorFacade
{
    public async Task ProcessMessagesAsync(int batchSize, int displacementMinute, CancellationToken cancellationToken)
    {
        List<OutboxMessageTypeGroup> groups = OutboxMessageTypeGrouping.GetGroups();

        foreach (OutboxMessageTypeGroup group in groups)
        {
            await iOutboxMessageProcessor.ProcessMessagesAsync(batchSize, displacementMinute, group.Types, cancellationToken);
        }
    }
}
