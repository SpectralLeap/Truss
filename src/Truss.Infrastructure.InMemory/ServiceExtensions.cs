using Truss.Modeling.Installation;

namespace Truss.Infrastructure.InMemory;

public static class ServiceExtensions
{
    public static TrussServiceConfiguration AddInMemoryEventSourcing(
        this TrussServiceConfiguration configuration
    )
    {
        return configuration.InstallModule<EventSourcingModule>();
    } 
}