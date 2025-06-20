namespace Inventory.SharedKernel;

public abstract class Entity
{
    protected Entity(long id)
    {
        Id = id;
    }

    public long Id { get; }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity &&
               GetType() == entity.GetType() &&
               Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }
}
