using Microsoft.Extensions.DependencyInjection;
using Web.Api.HostServices;

namespace Web.Api.Extensions;

public static class HostedExtensions
{
    public static IServiceCollection AddBackgroundHosted(this IServiceCollection services)
    {
        services.AddInventorHostedService();

        return services;
    }

    private static IServiceCollection AddInventorHostedService(this IServiceCollection services)
    {
        var interval = TimeSpan.FromSeconds(5);
        const int batchMessageCount = 50;

        services.AddSingleton<MessageProcessorTaskOptions>(new MessageProcessorTaskOptions(interval, batchMessageCount));

        services.AddHostedService<InventoryMessagesProcessorTask>();

        return services;
    }
}
