using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Infrastructure.InMemory.Events;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Application.Installation;
using Truss.Modeling.Installation;

namespace Truss.Infrastructure.InMemory;

public static class ServiceExtensions
{
    public static TrussServiceConfiguration AddInMemoryEventHandling(
        this TrussServiceConfiguration configuration
    )
    {
        return configuration.InstallModule<Module>();
    } 
}

internal sealed class Module : IModule
{
    public void Define(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventStore, InMemoryEventStore>();
    }
}
