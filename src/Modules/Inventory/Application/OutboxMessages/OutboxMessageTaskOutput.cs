using Inventory.SharedKernel;

namespace Inventory.Application.OutboxMessages;

public class OutboxMessageTaskOutput
{
    public OutboxMessageTaskOutput(
        long id,
        long entityId,
        string messageType,
        string content)
    {
        Id = id;
        EntityID = entityId;
        MessageType = Enum.Parse<OutboxMessageType>(messageType);
        Content = content;
    }

    public long Id { get; set; }
    public long EntityID { get; set; }
    public OutboxMessageType MessageType { get; set; }
    public string Content { get; set; }
}
