namespace Inventory.SharedKernel;

public interface IDateTimeProvider
{
    DateTime ServerNow { get; }
}
