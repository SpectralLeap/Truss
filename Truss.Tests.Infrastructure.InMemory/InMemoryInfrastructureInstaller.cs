using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Infrastructure;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public sealed class InMemoryInfrastructureInstaller 
    : ITrussInfrastructureInstaller
{
    public void Install(IServiceCollection services, TrussConfig config)
    {
        config.WithEventStore<InMemoryEventStore>();
    }
}