using Inventory.SharedKernel;

namespace Inventory.Application.OutboxMessages;

public interface IOutboxMessageHandler
{
    OutboxMessageType MessageType { get; }
    Task HandleAsync(OutboxMessageTaskOutput message, CancellationToken cancellationToken);
}
