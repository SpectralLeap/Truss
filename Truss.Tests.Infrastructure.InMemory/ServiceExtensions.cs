using Microsoft.Extensions.DependencyInjection;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public static class ServiceExtensions
{
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        var store = new InMemoryEventStore();
        return services
                .AddSingleton<IEventWriteStore>(store)
                .AddSingleton<IEventReadStore>(store)
            ;
    }
}