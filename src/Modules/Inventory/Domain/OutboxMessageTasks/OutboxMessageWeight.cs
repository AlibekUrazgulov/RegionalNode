namespace Inventory.Domain.OutboxMessageTasks;

public class OutboxMessageWeight
{
    public string TaskName { get; set; }
    public short WeightInPercent { get; set; }
}
