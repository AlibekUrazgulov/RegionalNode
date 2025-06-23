using Inventory.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.OutboxMessages.MessageHandlers;

public class FakeOutboxMessageHandler(ILogger<FakeOutboxMessageHandler> logger) : IOutboxMessageHandler
{
    public OutboxMessageType MessageType => OutboxMessageType.Fake;

    public async Task HandleAsync(OutboxMessageTaskOutput message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Call handle for {OutboxMessageType}", MessageType);

        await Task.CompletedTask;
    }
}
