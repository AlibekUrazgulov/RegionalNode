using Inventory.Application.OutboxMessageTasks;

namespace Inventory.Application.Abstractions.Data;

public interface IFinanceUnitOfWork : IUnitOfWork
{
    IOutboxMessageWeightRepository OutboxMessageWeightRepository { get; }
    IOutboxMessageRepository OutboxMessageRepository { get; }
}
