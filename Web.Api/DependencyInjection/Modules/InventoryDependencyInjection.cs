using Inventory.Application;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Endpoint;
using Inventory.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Web.Api.DependencyInjection.Modules;

public static class InventoryDependencyInjection
{
    public static IServiceCollection AddInventory(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoint()
                .AddApplication()
                .AddInfrastructure(configuration);

        return services;
    }
}
