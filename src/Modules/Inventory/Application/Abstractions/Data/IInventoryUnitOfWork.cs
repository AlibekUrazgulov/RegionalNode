using Inventory.Application.OutboxMessages;

namespace Inventory.Application.Abstractions.Data;

public interface IInventoryUnitOfWork : IUnitOfWork
{
    IOutboxMessageQueryRepository OutboxMessageQueryRepository { get; }
}
