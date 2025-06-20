using System.Reflection;
using System.Windows.Input;
using Inventory.Domain.OutboxMessageTasks;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(OutboxMessageTask).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    // protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    // protected static readonly Assembly EndpointAssembly = typeof(Inventory.Endpoint.DependencyInjection).Assembly;
}
