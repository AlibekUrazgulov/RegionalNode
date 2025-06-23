using System.Reflection;

namespace Inventory.SharedKernel;

[AttributeUsage(AttributeTargets.Field)]
public sealed class SlowOutboxMessageAttribute : Attribute { }

public enum OutboxMessageType
{
    Fake = 0,
}

public class OutboxMessageTypeGroup
{
    private OutboxMessageTypeGroup(string name, List<OutboxMessageType> outboxMessageTypes)
    {
        Name = name;
        Types = outboxMessageTypes;
    }

    public string Name { get; set; }
    public List<OutboxMessageType> Types { get; set; }

    public static OutboxMessageTypeGroup CreateSlow(OutboxMessageType messageType)
    {
        return new OutboxMessageTypeGroup($"Slow_{messageType}", [messageType]);
    }

    public static OutboxMessageTypeGroup CreateDefault(List<OutboxMessageType> messageTypes)
    {
        return new OutboxMessageTypeGroup($"Default", messageTypes);
    }
}

public static class OutboxMessageTypeGrouping
{
    public static List<OutboxMessageTypeGroup> GetGroups()
    {
        var groups = new List<OutboxMessageTypeGroup>();

        _ = new List<OutboxMessageType>();
        var otherTypes = new List<OutboxMessageType>();

        foreach (OutboxMessageType type in Enum.GetValues<OutboxMessageType>())
        {
            if (ItSlow(type))
            {
                groups.Add(OutboxMessageTypeGroup.CreateSlow(type));
            }
            else if (type != 0)
            {
                otherTypes.Add(type);
            }
        }

        if (otherTypes.Count > 0)
        {
            groups.Add(OutboxMessageTypeGroup.CreateDefault(otherTypes));
        }

        return groups;
    }

    private static bool ItSlow(OutboxMessageType type)
    {
        MemberInfo? member = typeof(OutboxMessageType).GetMember(type.ToString()).FirstOrDefault();
        return member != null && member.GetCustomAttribute<SlowOutboxMessageAttribute>() != null;
    }
}
