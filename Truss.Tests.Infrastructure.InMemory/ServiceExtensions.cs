using Microsoft.Extensions.DependencyInjection;
using Truss.Application.Cqrs.EventSourcing.Common;
using Truss.Tests.Infrastructure.InMemory.Events;

namespace Truss.Tests.Infrastructure.InMemory;

public static class ServiceExtensions
{
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        return services
                .AddSingleton<IEventStore, InMemoryEventStore>()
            ;
    }
}