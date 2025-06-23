using Inventory.SharedKernel;

namespace Inventory.Domain.OutboxMessageTasks;

public class OutboxMessageTask : Entity
{
    private OutboxMessageTask(long id, string messageTypeName, string content, DateTime occurredOn, DateTime plannedProcessDate)
        : base(id)
    {
        MessageTypeName = messageTypeName;
        Content = content;
        OccurredOn = occurredOn;
        PlannedProcessDate = plannedProcessDate;
        AttemptCount = 0;
    }

    public OutboxMessageTask(string messageTypeName, string content, DateTime occurredOn, DateTime plannedProcessDate)
        : base(0)
    {
        MessageTypeName = messageTypeName;
        Content = content;
        OccurredOn = occurredOn;
        PlannedProcessDate = plannedProcessDate;
        AttemptCount = 0;
    }

    public long EntityID { get; }
    public string MessageTypeName { get; }
    public string Content { get; }
    public DateTime OccurredOn { get; }
    public DateTime PlannedProcessDate { get; }
    public string Error { get; private set; }
    public int AttemptCount { get; private set; }

    public static OutboxMessageTask Create(
        long entityId,
        string messageTypeName,
        string content,
        DateTime occurredOn,
        DateTime plannedProcessDate)
    {
        if (string.IsNullOrWhiteSpace(messageTypeName))
        {
            throw new ArgumentException("Message type name cannot be null or empty.", nameof(messageTypeName));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        }

        return new OutboxMessageTask(entityId, messageTypeName, content, occurredOn, plannedProcessDate);
    }

    public void WithError(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            throw new ArgumentException("Error message cannot be null or empty.", nameof(error));
        }

        Error = error;
        AttemptCount++;
    }
}
