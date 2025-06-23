using Inventory.Application.Abstractions.Authentication;
using Inventory.Application.Abstractions.Data;
using Inventory.Infrastructure.Authentication;
using Inventory.Infrastructure.Database;
using Inventory.Infrastructure.DomainEvents;
using Inventory.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    private const string HospitalConnectionStringKey = "appSettings:Finance_Connection";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IFormatProvider>(System.Globalization.CultureInfo.InvariantCulture);

        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetSection(HospitalConnectionStringKey)?.Value ?? string.Empty;

        services.AddDbContext<InventoryDbContext>(
            options => options
                .UseSqlServer(connectionString));

        services.AddSingleton<IConnectionStringProvider>(new InventoryConnectionStringProvider(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<InventoryUnitOfWork>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetSection(HospitalConnectionStringKey)?.Value ?? string.Empty;

        services
            .AddHealthChecks()
            .AddSqlServer(connectionString);

        return services;
    }
}
