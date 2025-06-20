using Inventory.SharedKernel;

namespace Inventory.Infrastructure.Time;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime ServerNow => DateTime.Now;
}
