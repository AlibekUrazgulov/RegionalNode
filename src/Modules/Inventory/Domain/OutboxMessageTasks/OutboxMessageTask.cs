using Inventory.SharedKernel;

namespace Inventory.Domain.OutboxMessageTasks;

public class OutboxMessageTask : Entity
{
    public OutboxMessageTask(long id) : base(id)
    {
    }
}
