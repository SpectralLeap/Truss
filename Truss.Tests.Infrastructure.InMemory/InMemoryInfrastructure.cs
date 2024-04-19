using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Truss.Modeling.Infrastructure;
using Truss.Modeling.Infrastructure.Installation;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public sealed class InMemoryInfrastructure 
    : IInfrastructure
{
    public void Define(
        IServiceCollection services,
        TrussServiceConfiguration serviceConfiguration,
        IConfiguration configuration
    )
    {
        serviceConfiguration.SetEventStore<InMemoryEventStore>();
    }
}