using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Application.Cqrs.EventSourcing.Reading;
using Truss.Modeling.Application.Cqrs.EventSourcing.Writing;
using Truss.Modeling.Infrastructure;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public sealed class InMemoryInfrastructureServiceInstaller 
    : ITrussServiceInstaller
{
    public void InstallServices(IServiceCollection services)
    {
        var store = new InMemoryEventStore();
        
        services
            .AddSingleton<IEventWriteStore>(store)
            .AddSingleton<IEventReadStore>(store)
            ;
    }
}