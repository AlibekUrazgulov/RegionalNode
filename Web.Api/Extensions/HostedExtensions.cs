using Microsoft.Extensions.DependencyInjection;
using Web.Api.HostServices;

namespace Web.Api.Extensions;

public static class HostedExtensions
{
    public static IServiceCollection AddBackgroundHosted(this IServiceCollection services)
    {
        services.AddHostedService<MessagesProcessorTask>();
        services.AddHostedService<MessagesProcessorTask>();

        return services;
    }
}
