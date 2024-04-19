using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Infrastructure;
using Truss.Modeling.Infrastructure.Installation;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public sealed class InMemoryInfrastructureInstaller 
    : ITrussInfrastructureInstaller
{
    public void Install(
        IServiceCollection services,
        TrussServiceConfiguration serviceConfiguration,
        IConfiguration configuration
    )
    {
        serviceConfiguration.SetEventStore<InMemoryEventStore>();
    }
}